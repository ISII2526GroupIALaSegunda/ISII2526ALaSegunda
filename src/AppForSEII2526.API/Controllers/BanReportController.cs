using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.BanUserDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BanReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBanReport([FromBody] BanReportForCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var banReport = new BanReport
            {
                Reason = dto.Reason,
                DetailedDescription = dto.DetailedDescription,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ReportCustomers = new List<ReportCustomer>()
            };

            foreach (var customer in dto.Customers)
            {
                banReport.ReportCustomers.Add(new ReportCustomer
                {
                    CustomerId = customer.CustomerId,
                    Message = customer.Message,
                    State = 0
                });
            }

            _context.BanReports.Add(banReport);
            await _context.SaveChangesAsync();

           
            var result = new BanReportDTO
            {
                Id = banReport.ID,
                Reason = banReport.Reason,
                DetailedDescription = banReport.DetailedDescription,
                StartDate = banReport.StartDate,
                EndDate = banReport.EndDate,
                Customers = banReport.ReportCustomers.Select(rc => new ReportCustomerDTO
                {
                    CustomerId = rc.CustomerId,
                    Message = rc.Message,
                    State = (int)rc.State
                }).ToList()
            };

            return Ok(result);
        }
    }
}