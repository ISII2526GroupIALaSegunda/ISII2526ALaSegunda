namespace AppForSEII2526.API.DTOs.DeliveryAssignmentDTOs
{
    public class DeliveryAssignmentForCreateDTO {
        public DeliveryAssignmentForCreateDTO(int deliveryDriverId, DateTime deliveryAssignmentDone, string personalMessage, decimal extraReward, IList<PurchaseOrderForAssignmentDTO> purchaseOrders)
        {
            DeliveryDriverId = deliveryDriverId;
            DeliveryAssignmentDone = deliveryAssignmentDone;
            PersonalMessage = personalMessage;
            ExtraReward = extraReward;
            PurchaseOrders = purchaseOrders;
        }

        public int DeliveryDriverId { get; set; }

        public DateTime DeliveryAssignmentDone { get; set; }

        [StringLength(20, ErrorMessage = "Personal Message can be neither longer than 200 characters")]
        public string PersonalMessage { get; set; }

        [Precision(10, 2)]
        [Range(0, 100, ErrorMessage = "Extra reward must be between 0 and 100€")]
        public decimal ExtraReward { get; set; }

        [Required(ErrorMessage = "At least one purchase order must be selected")]
        [MinLength(1, ErrorMessage = "At least one purchase order must be selected")]
        public IList<PurchaseOrderForAssignmentDTO> PurchaseOrders { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DeliveryAssignmentForCreateDTO dTO &&
                   DeliveryDriverId == dTO.DeliveryDriverId &&
                   DeliveryAssignmentDone == dTO.DeliveryAssignmentDone &&
                   PersonalMessage == dTO.PersonalMessage &&
                   ExtraReward == dTO.ExtraReward &&
                   PurchaseOrders.SequenceEqual(dTO.PurchaseOrders);
        }
    }
}