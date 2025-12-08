using AppForSEII2526.API.DTOs_PurchaseProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseProductsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchaseProductsController> _logger;

        public PurchaseProductsController(ApplicationDbContext context, ILogger<PurchaseProductsController> logger)
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
        [ProducesResponseType(typeof(IList<PurchaseProductsForReturningDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ModelError), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetPurchaseProductsForReturning(string? productName, int quantity, string userName
            ){ 
            
            //if(!isReturnable)
            //{
            //    ModelState.AddModelError("isReturnable", "Product has already been returned");
            //    _logger.LogError($"Product must have not been returned yet");
            //    return BadRequest(new ValidationProblemDetails(ModelState));
            //}

            IList<PurchaseProductsForReturningDTO> purchaseProductsDTOS = await _context.PurchaseProducts
                
                .Include(purchaseProduct => purchaseProduct.Product)
                .Include(purchaseProduct => purchaseProduct.ReturnProduct)
                .Include(purchaseProduct => purchaseProduct.Product.Brand)
                .Include(p => p.PurchaseOrder).ThenInclude(po => po.Customer)

                .Where(purchaseProduct=>(purchaseProduct.Product.IsReturnable)
                
                &&(purchaseProduct.PurchaseOrder.Customer.UserName == userName)

                &&(purchaseProduct.ReturnProduct == null))

                .Select(purchaseProduct=>new PurchaseProductsForReturningDTO(purchaseProduct.ProductId, purchaseProduct.Product.Name,
                purchaseProduct.Quantity, purchaseProduct.Product.Brand.Name, purchaseProduct.Product.Brand.Location, purchaseProduct.Product.IsReturnable))
                .ToListAsync();
            return Ok(purchaseProductsDTOS);
        }
    
    }
}
