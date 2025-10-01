namespace AppForSEII2526.API.Models
{
    public class DeliveryAssignment
    {
        public int Id { get; set; }
        public DateTime DeliveryAssignmentDone { get; set; }
        [Precision(5,2)]
        public decimal ExtraReward { get; set; }

        [StringLength(20, ErrorMessage = "Personal Message can be neither longer than 200 characters")]
        public string PersonalMessage { get; set; }

        public IList<PurchaseDelivery> PurchaseDeliveries { get; set; }

        public DeliveryDriver DeliveryMan { get; set; }
    }
}
