namespace AppForSEII2526.API.DTOs.DeliveryAssignmentDTOs
{
    public class DeliveryAssignmentForDetailDTO
    {
        public DeliveryAssignmentForDetailDTO(int id, string deliveryDriverName, DateTime deliveryAssignmentDone, string personalMessage, decimal extraReward, IList<PurchaseDeliveryForDetailDTO> purchaseDeliveries)
        {
            Id = id;
            DeliveryDriverName = deliveryDriverName;
            DeliveryAssignmentDone = deliveryAssignmentDone;
            PersonalMessage = personalMessage;
            ExtraReward = extraReward;
            PurchaseDeliveries = purchaseDeliveries;
        }
        public int Id { get; set; }
        [StringLength(20, ErrorMessage = "Name can not be longer than 200 characters")]
        public string DeliveryDriverName { get; set; }
        public DateTime DeliveryAssignmentDone { get; set; }
        [Precision(5, 2)]
        public decimal ExtraReward { get; set; }

        [StringLength(20, ErrorMessage = "Personal Message can be neither longer than 200 characters")]
        public string PersonalMessage { get; set; }

        public IList<PurchaseDeliveryForDetailDTO> PurchaseDeliveries { get; set; }

    }
}
