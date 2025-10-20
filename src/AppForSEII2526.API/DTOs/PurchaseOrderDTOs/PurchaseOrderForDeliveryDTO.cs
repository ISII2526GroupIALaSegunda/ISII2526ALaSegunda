namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseOrderForDeliveryDTO
    {
        public PurchaseOrderForDeliveryDTO(int id, DateTime date, decimal totalprice, string city, string street, string postalcode)
        {
            Id = id;
            Date = date;
            TotalPrice = totalprice;
            City = city;
            Street = street;
            PostalCode = postalcode;
        }
        public int Id { get; set; }
        [Required, StringLength(120)]
        public DateTime Date { get; set; }

        [Precision(10, 2)]
        [Range(typeof(decimal), "0.00", "9999999999.99")]
        public decimal TotalPrice { get; set; }

        [Required, StringLength(60)]
        public string City { get; set; }

        [Required, StringLength(120)]
        public string Street { get; set; }

        [Required, StringLength(20)]
        public string PostalCode { get; set; }
    }
}
