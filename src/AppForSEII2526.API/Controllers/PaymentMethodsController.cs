using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.PaymentDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentMethodsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<PaymentMethodDTO>>> GetMyPaymentMethods()
        {
           
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(new { message = "User not identified" });

           
            var methods = await _context.PaymentMethods
                .Where(pm => pm.User.Id == user.Id)
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