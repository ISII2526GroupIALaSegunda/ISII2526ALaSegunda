namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeliveryAssignmentId),nameof(PurchaseOrderId))]
    public class PurchaseDelivery
    {
        public PurchaseDelivery(DateTime date, PriorityType priority, DeliveryAssignment deliveryAssignment)
        {
            Date = date;
            Priority = priority;
            DeliveryAssignment = deliveryAssignment;
        }

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
