using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            //Context for accesing to the database
            _context = context;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{
        //    if (op2 == 0)
        //    {
        //        _logger.LogError($"{DateTime.Now} Exception: op2=0, division by 0");
        //        return BadRequest("op2 must be different from 0");
        //    }
        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}


        //GET method
        [HttpGet]
        [Route("[action]")]
        //What I'm going to return
        [ProducesResponseType(typeof(IList<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProductsForPurchasing()
        {
            IList<Product> products = await _context.Products
                .ToListAsync();
            return Ok(products);
        }
    }

   

}
