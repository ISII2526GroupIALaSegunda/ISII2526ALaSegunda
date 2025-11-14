namespace AppForSEII2526.API.DTOs.ReturnProductsDTOs
{
    public class ReturnForCreateDTO
    {
        public ReturnForCreateDTO(
            int purchaseOrderId,
            string userNameCustomer,
            string reason,
            int paymentMethod,
            int? rating,
            List<ReturnItemForCreateDTO> returnItems)
        {
            PurchaseOrderId = purchaseOrderId;
            UserNameCustomer = userNameCustomer;
            Reason = reason;
            PaymentMethod = paymentMethod;
            Rating = rating;
            ReturnItems = returnItems;
        }

        public int PurchaseOrderId { get; set; }
        public string UserNameCustomer { get; set; }  

        public string Reason { get; set; }            
        public int PaymentMethod { get; set; }     
        public int? Rating { get; set; }              

        public List<ReturnItemForCreateDTO> ReturnItems { get; set; }
    }
}
