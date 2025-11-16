using AppForSEII2526.API.DTOs.BanUserDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BanController> _logger;

        public BanController(ApplicationDbContext context, ILogger<BanController> logger) {
            _context = context;
            _logger = logger;

        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(BanDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBanReport(int id) {
            if (_context.BanReports == null)
            {
                _logger.LogError("Error: BanReports table does not exist");
                return NotFound();
            }

            var banReport = await _context.BanReports
                .Where(r => r.ID == id) 
                .Include(r => r.ReportCustomers)
                    .ThenInclude(rc => rc.ApplicationCustomer)
                .Select(r => new BanDetailDTO(

                    r.ID,
                    r.Reason,
                    r.DetailedDescription,
                    r.StartDate,
                    r.EndDate,
                    "In progress",
                    r.ReportCustomers.Select(rc => new ReportCustomerForDetailDTO(
                            rc.CustomerId,
                            rc.ApplicationCustomer.Name,
                            rc.ApplicationCustomer.Surname,
                            rc.Message
                        )).ToList()
                ))
                    .FirstOrDefaultAsync();

            if (banReport == null)
            {
                _logger.LogError($"Error: BanReport with id {id} does not exist");
                return NotFound();
            }
            return Ok(banReport);


        }




    }
}
