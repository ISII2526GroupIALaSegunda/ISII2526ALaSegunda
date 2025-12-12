using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.PaymentDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentMethodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<PaymentMethodDTO>>> GetMyPaymentMethods()
        {
            string userName = User.Identity.Name;

            var methods = await _context.PaymentMethods
                .Include(pm => pm.User)
                .Where(pm => pm.User.UserName == userName)
                .Select(pm => new PaymentMethodDTO
                {
                    Id = pm.Id,
                    Description = pm is CreditCard
                        ? $"Credit Card (Ends in {((CreditCard)pm).CreditCardNumber.Substring(((CreditCard)pm).CreditCardNumber.Length - 4)})"
                        : $"Bizum ({((Bizum)pm).TelephoneNumber})"
                })
                .ToListAsync();

            return Ok(methods);
        }
    }
}
