
namespace AppForSEII2526.API.Models
{
    public class DeliveryAssignment
    {
        public DeliveryAssignment()
        {
        }

        public DeliveryAssignment(string personalMessage, decimal extraReward, DeliveryDriver? deliveryDriver, DateTime deliveryAssignmentDone, List<PurchaseDelivery> purchaseDeliveries)
        {
            PersonalMessage = personalMessage;
            ExtraReward = extraReward;
            DeliveryMan = deliveryDriver;
            DeliveryAssignmentDone = deliveryAssignmentDone;
            PurchaseDeliveries = purchaseDeliveries;
        }

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
