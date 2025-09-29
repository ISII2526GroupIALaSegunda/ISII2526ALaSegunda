namespace AppForSEII2526.API.Models
{
    public class PurchaseDelivery
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public DeliveryAssignment DeliveryAssignment { get; set; }

        public PriorityType Priority { get; set; }

    }

    public enum PriorityType
    {
        Low,
        Medium,
        High
    }
}
