namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseOrderForAssignmentDTO{
        public PurchaseOrderForAssignmentDTO(int purchaseOrderId, PriorityType priority, int id)
        {
            PurchaseOrderId = purchaseOrderId;
            Priority = priority;
            Id = id;
        }

        [Required(ErrorMessage = "Purchase order ID is required")]
        public int PurchaseOrderId { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public PriorityType Priority { get; set; }

        public int Id { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseOrderForAssignmentDTO dTO &&
                   PurchaseOrderId == dTO.PurchaseOrderId &&
                   Priority == dTO.Priority &&
                   Id == dTO.Id;
        }
    }
}