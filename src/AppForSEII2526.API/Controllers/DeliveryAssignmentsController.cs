using AppForSEII2526.API.DTOs.DeliveryAssignmentDTOs;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAssignmentsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<DeliveryAssignmentsController> _logger;
        public DeliveryAssignmentsController(ApplicationDbContext context, ILogger<DeliveryAssignmentsController> logger)
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
                        .Select(pd => new PurchaseDeliveryDTO(
                            pd.Date,
                            pd.PurchaseOrder.Street,
                            pd.PurchaseOrder.City,
                            pd.PurchaseOrder.PostalCode,
                            pd.PurchaseOrder.TotalPrice,
                            pd.Priority,
                            pd.PurchaseOrderId 
                            ))
                        .ToList())
                )
                //it obtains just the first DeliveryAssignment that satisfies the where clause
                .FirstOrDefaultAsync();
                
                return Ok(deliveryAssignment);
        }

        [HttpPost]
        [Route("[action]")]

        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(DeliveryAssignmentForDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateDeliveryAssignment(DeliveryAssignmentForCreateDTO deliveryAssignmentForCreate){
            if (deliveryAssignmentForCreate.DeliveryAssignmentDone <= DateTime.Now)
                ModelState.AddModelError("DeliveryAssignmentDone", "Error! Delivery deadline must be later than now");

            if (deliveryAssignmentForCreate.PurchaseDeliveries.Count == 0)
                ModelState.AddModelError("PurchaseDeliveries", "Error! You must include at least one purchase order to be delivered");

            if (!(deliveryAssignmentForCreate.PersonalMessage.StartsWith("Please,")))
            {
                string error = "Error!, You must start personale message with Please,";
                ModelState.AddModelError("PersonalMessage", error);
                return BadRequest(error);
            }

            var deliveryDriver = await _context.DeliveryDrivers.FirstOrDefaultAsync(dd => dd.id == deliveryAssignmentForCreate.DeliveryDriverId);
            if (deliveryDriver == null)
                ModelState.AddModelError("DeliveryDriver", "Error! Delivery driver does not exist");

            var purchaseOrdersIds = deliveryAssignmentForCreate.PurchaseDeliveries.Select(pd => pd.PurchaseOrderId).ToList<int>();

            var purchaseOrders = _context.PurchaseOrders.Include(po => po.DriverAssigned)
                .ThenInclude(pd => pd.DeliveryAssignment)
                .Where(po => purchaseOrdersIds.Contains(po.Id))
                .Select(po => new
                {
                    po.Id,
                    po.City,
                    po.Street,
                    po.PostalCode,
                    po.TotalPrice,
                })
                .ToList();

            DeliveryAssignment deliveryAssignment = new DeliveryAssignment(deliveryAssignmentForCreate.PersonalMessage, deliveryAssignmentForCreate.ExtraReward, deliveryDriver, deliveryAssignmentForCreate.DeliveryAssignmentDone, new List<PurchaseDelivery>());

            foreach (var item in deliveryAssignmentForCreate.PurchaseDeliveries)
            {
                var purchaseOrder = purchaseOrders.FirstOrDefault(po => po.Id == item.PurchaseOrderId);
                if (purchaseOrder != null)
                {
                    ModelState.AddModelError("PurchaseOrders", $"PurchaseOrder with id {item.PurchaseOrderId} does not exist.");
                }
                else
                {
                    deliveryAssignment.PurchaseDeliveries.Add(new PurchaseDelivery(item.Date, item.Priority, deliveryAssignment));
                }
            }

            if (deliveryDriver == null)
                ModelState.AddModelError("DeliveryDriver", "Error! Delivery driver does not exist");

            _context.Add(deliveryAssignment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now + " Error: " + ex.Message);
                return Conflict("Error" + ex.Message);
            }

            var deliveryAssignmentDetail = new DeliveryAssignmentForDetailDTO(deliveryAssignment.Id ,deliveryAssignment.DeliveryMan.Name!,
                deliveryAssignment.DeliveryAssignmentDone, deliveryAssignment.PersonalMessage, deliveryAssignment.ExtraReward, deliveryAssignmentForCreate.PurchaseDeliveries);

            return CreatedAtAction("GetDeliveryAssignment", new { id = deliveryAssignment.Id }, deliveryAssignmentDetail);

        }
    }
}