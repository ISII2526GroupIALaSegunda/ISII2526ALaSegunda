using AppForSEII2526.API.DTOs.DeliveryAssignmentDTOs;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAssignmentsController : Controller
    {
        private ApplicationDbContext _context;
        private ILogger<PurchaseOrdersController> _logger;
        public DeliveryAssignmentsController(ApplicationDbContext context, ILogger<PurchaseOrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DeliveryAssignmentForDetailDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDeliveryAssignment(int id)
        {
            //perhaps it does not exist a DeliveryAssignment with the id provided
            DeliveryAssignmentForDetailDTO? deliveryAssignment = await _context.DeliveryAssignments
                .Where(da => da.Id == id)

                .Include(da => da.DeliveryMan) //join table DeliveryDriver

                .Include(da => da.PurchaseDeliveries) //join table PurchaseDeliveries
                    .ThenInclude(pd => pd.PurchaseOrder) //then join table PurchaseOrder

                .Select(da => new DeliveryAssignmentForDetailDTO(
                    da.Id,
                    //we add the null forgiving operator, ! to the right-hand side to avoid the warning "maybe-null"
                    da.DeliveryMan.Name!,
                    da.DeliveryAssignmentDone,
                    da.PersonalMessage,
                    da.ExtraReward,
                    da.PurchaseDeliveries
                        .Select(pd => new PurchaseDeliveryForDetailDTO(
                            pd.Date,
                            pd.PurchaseOrder.Street,
                            pd.PurchaseOrder.City,
                            pd.PurchaseOrder.PostalCode,
                            pd.PurchaseOrder.TotalPrice,
                            pd.Priority))
                        .ToList())
                )
                //it obtains just the first DeliveryAssignment that satisfies the where clause
                .FirstOrDefaultAsync();
                
                return Ok(deliveryAssignment);
        }
    }
}