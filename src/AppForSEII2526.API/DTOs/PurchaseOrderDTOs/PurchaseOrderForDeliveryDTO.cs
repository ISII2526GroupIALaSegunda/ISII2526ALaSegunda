namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseOrderForDeliveryDTO
    {
        public PurchaseOrderForDeliveryDTO(int id, DateTime date, decimal totalprice)
        {
            Id = id;
            Date = date;
            TotalPrice = totalprice;
        }
        public int Id { get; set; }
        [Required, StringLength(120)]
        public DateTime Date { get; set; }

        [Precision(10, 2)]
        [Range(typeof(decimal), "0.00", "9999999999.99")]
        public decimal TotalPrice { get; set; }
    }
}
