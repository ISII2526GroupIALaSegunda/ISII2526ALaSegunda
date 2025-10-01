namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeliveryAssignmentId),nameof(PurchaseOrderId))]
    public class PurchaseDelivery
    {
        public int DeliveryAssignmentId { get; set; }
        public int PurchaseOrderId { get; set; }

        public DateTime Date { get; set; }

        public DeliveryAssignment DeliveryAssignment { get; set; }

        public PriorityType Priority { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }

    }

    public enum PriorityType
    {
        Low,
        Medium,
        High
    }
}
