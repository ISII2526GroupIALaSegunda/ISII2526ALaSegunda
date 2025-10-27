using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseForDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            var purchase = await _context.PurchaseOrders
                .Where(po => po.Id == id)

                .Include(po => po.PurchaseProducts)        
                    .ThenInclude(pp => pp.Product)          
                        .ThenInclude(p => p.Brand)        

                .Include(po => po.Customer)                 
                .Include(po => po.PaymentMethod)           

                .Select(po => new PurchaseForDetailDTO(
                    po.Id,
                    po.TotalPrice,
                    po.Date,
                    po.Street,
                    po.City,
                    po.PostalCode,
                    po.NameSurname,
                    po.State.ToString(), // enum -> string
                    EF.Property<string>(po.PaymentMethod, "Discriminator"),
                    po.Customer.UserName!, 
                    po.PurchaseProducts
                        .Select(pp => new PurchaseItemDTO(
                            pp.ProductId,
                            pp.Product.Name,
                            pp.Product.Brand.Name,
                            pp.Product.Colour,
                            pp.Price,          
                            pp.Quantity
                        )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }

            return Ok(purchase);
        }
    }
}