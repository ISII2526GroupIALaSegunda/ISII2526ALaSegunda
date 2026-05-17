using AppForSEII2526.API.DTOs.ReturnProductsDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnProductsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ReturnProductsController> _logger;

        public ReturnProductsController(ApplicationDbContext context, ILogger<ReturnProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int) HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{

        //    if (op2 == 0)
        //    {
        //        string error = "Op2 cannot be 0 to compute a division";
        //        _logger.LogError(DateTime.Now + "Error:"+ error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReturnSummaryDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetReturnSummary(int returnPurchaseOrderId)
        {
            ReturnSummaryDTO? summary = await _context.Returns

                // JOIN
                .Include(rpo => rpo.Customer)
                .Include(rpo => rpo.ReturnProducts)
                    .ThenInclude(rp => rp.PurchaseProduct)
                        .ThenInclude(pp => pp.Product)
                            .ThenInclude(p => p.Brand)

                .Where(rpo => rpo.Id == returnPurchaseOrderId)

                .Select(rpo => new ReturnSummaryDTO(



                    // Customer
                    rpo.Customer.Name,
                    rpo.Customer.Surname,
                    rpo.Customer.Address,
                    rpo.Customer.PhoneNumber,

                    // Products list
                    rpo.ReturnProducts.Select(rp => new ReturnedProductInfoDTO(
                        rp.Quantity,
                        rp.PurchaseProduct.Product.Name,
                        rp.PurchaseProduct.Product.Brand.Name,
                        rp.PurchaseProduct.Product.Brand.Location
                       
                    )).ToList()
                ))

                .FirstOrDefaultAsync();

            if (summary == null)
                return NotFound("Return not found.");

            return Ok(summary);
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ReturnSummaryDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateReturn(ReturnForCreateDTO returnForCreate)
        {
            // --- Validaciones básicas (Alt Flow 5) ---

            if (string.IsNullOrWhiteSpace(returnForCreate.Reason))
                ModelState.AddModelError("Reason", "Error! Reason is mandatory.");

            if (returnForCreate.PaymentMethodId <= 0)
                ModelState.AddModelError("PaymentMethodId", "Error! Payment method is mandatory.");

            if (returnForCreate.ReturnItems == null || returnForCreate.ReturnItems.Count == 0)
                ModelState.AddModelError("ReturnItems", "Error! You must include at least one product to be returned.");

            if (returnForCreate.Rating.HasValue &&
                (returnForCreate.Rating.Value < 1 || returnForCreate.Rating.Value > 5))
                ModelState.AddModelError("Rating", "Error! Rating must be between 1 and 5.");

            // Usuario (similar a Rental: relacionar con ApplicationUser)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == returnForCreate.UserNameCustomer);

            if (user == null)
                ModelState.AddModelError("ReturnApplicationUser", "Error! UserName is not registered.");

            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));


            // --- Cargamos el PaymentMethod por Id ---

            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(pm => pm.Id == returnForCreate.PaymentMethodId);

            if (paymentMethod == null)
            {
                ModelState.AddModelError("PaymentMethodId", "Error! Payment method not found.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }


            // --- Cargamos el PurchaseOrder con sus PurchaseProducts y Product+Brand ---

            var purchaseOrder = await _context.PurchaseOrders
                .Include(po => po.Customer)
                .Include(po => po.PurchaseProducts)
                    .ThenInclude(pp => pp.Product)
                        .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync(po => po.Id == returnForCreate.PurchaseOrderId);

            if (purchaseOrder == null)
            {
                ModelState.AddModelError("PurchaseOrderId", "Error! Purchase order not found.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Comprobación extra de coherencia usuario–pedido
            if (purchaseOrder.Customer.UserName != returnForCreate.UserNameCustomer)
            {
                ModelState.AddModelError("UserNameCustomer", "Error! The order does not belong to the given user.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }


            // --- Preparamos la ReturnPurchaseOrder (todavía sin guardar) ---

            var returnPurchaseOrder = new ReturnPurchaseOrder
            {
                Name = $"RPO-{purchaseOrder.Id}-{DateTime.UtcNow:yyyyMMddHHmmss}",
                Date = DateTime.UtcNow,
                MoneyToReturn = 0m,                           // mejor decimal que double
                NewTotalPrice = purchaseOrder.TotalPrice,     // ajustaremos después
                TotalPrice = purchaseOrder.TotalPrice,
                Rating = returnForCreate.Rating,
                PaymentMethod = paymentMethod,                // entidad, no enum
                Customer = purchaseOrder.Customer,
                ReturnProducts = new List<ReturnProduct>()
            };

            decimal moneyToReturn = 0m;


            // --- Validamos y creamos cada ReturnProduct (Alt Flow 3) ---

            foreach (var item in returnForCreate.ReturnItems)
            {
                // recordemos: identificamos la línea por ProductId dentro del pedido
                var purchaseProduct = purchaseOrder.PurchaseProducts
                    .FirstOrDefault(pp => pp.ProductId == item.ProductId);

                if (purchaseProduct == null)
                {
                    ModelState.AddModelError("ReturnItems",
                        $"Error! ProductId {item.ProductId} does not belong to this order.");
                    continue;
                }

                // Restricción de empresa: el producto debe ser retornable
                if (!purchaseProduct.Product.IsReturnable)
                {
                    ModelState.AddModelError("ReturnItems",
                        $"Error! Product '{purchaseProduct.Product.Name}' cannot be returned due to company restrictions.");
                    continue;
                }

                // Cantidad ya devuelta antes de esta operación para ESTA línea (pedido + producto)
                int alreadyReturned = await _context.ReturnProducts
                    .Where(rp =>
                        rp.PurchaseProduct.PurchaseOrderId == purchaseOrder.Id &&
                        rp.PurchaseProduct.ProductId == item.ProductId)
                    .Select(rp => (int?)rp.Quantity)
                    .SumAsync() ?? 0;

                int maxReturnable = purchaseProduct.Quantity - alreadyReturned;

                if (maxReturnable <= 0)
                {
                    ModelState.AddModelError("ReturnItems",
                        $"Error! Product '{purchaseProduct.Product.Name}' has no remaining quantity to be returned.");
                    continue;
                }

                if (item.Quantity <= 0 || item.Quantity > maxReturnable)
                {
                    ModelState.AddModelError("ReturnItems",
                        $"Error! Product '{purchaseProduct.Product.Name}' cannot be returned in quantity {item.Quantity}. Max returnable: {maxReturnable}.");
                    continue;
                }

                // Creamos la línea ReturnProduct
                var returnProduct = new ReturnProduct
                {
                    PurchaseProduct = purchaseProduct,   // EF usa la FK compuesta (OrderId + ProductId)
                    Quantity = item.Quantity,
                    Reason = returnForCreate.Reason
                };

                returnPurchaseOrder.ReturnProducts.Add(returnProduct);

                // Dinero a devolver
                moneyToReturn += purchaseProduct.Price * item.Quantity;
            }

            // Si ha habido cualquier problema con las líneas → Alt Flow 3
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Actualizamos MoneyToReturn y NewTotalPrice
            returnPurchaseOrder.MoneyToReturn = moneyToReturn;
            returnPurchaseOrder.NewTotalPrice = purchaseOrder.TotalPrice - moneyToReturn;

            // Ajusta el nombre del DbSet si no es "Returns"
            _context.Returns.Add(returnPurchaseOrder);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving return");

                return Conflict("Error: " + (ex.InnerException?.Message ?? ex.Message));
            }


            // --- Construimos el DTO de detalle (summary) para la respuesta (paso 7) ---

            var returnedProductsDto = returnPurchaseOrder.ReturnProducts
                .Select(rp => new ReturnedProductInfoDTO(
                    rp.Quantity,                                    // según tu ctor actual
                    rp.PurchaseProduct.Product.Name,
                    rp.PurchaseProduct.Product.Brand.Name,
                    rp.PurchaseProduct.Product.Brand.Location
                ))
                .ToList();

            var returnDetail = new ReturnSummaryDTO(
                returnPurchaseOrder.Customer.Name,
                returnPurchaseOrder.Customer.Surname,
                returnPurchaseOrder.Customer.Address,
                returnPurchaseOrder.Customer.PhoneNumber,
                returnedProductsDto
            );

            return CreatedAtAction("GetReturnSummary",
                new { returnPurchaseOrderId = returnPurchaseOrder.Id },
                returnDetail);
        }



    }
}
