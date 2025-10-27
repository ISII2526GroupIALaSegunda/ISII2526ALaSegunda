namespace AppForSEII2526.API.DTOs.DeliveryAssignmentDTOs
{
    public class PurchaseOrderForAssignmentDTO{
        public PurchaseOrderForAssignmentDTO(int purchaseOrderId, PriorityType priority)
        {
            PurchaseOrderId = purchaseOrderId;
            Priority = priority;
        }

        [Required(ErrorMessage = "Purchase order ID is required")]
        public int PurchaseOrderId { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public PriorityType Priority { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseOrderForAssignmentDTO dTO &&
                   PurchaseOrderId == dTO.PurchaseOrderId &&
                   Priority == dTO.Priority;
        }
    }
}