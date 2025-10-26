namespace AppForSEII2526.API.DTOs.DeliveryAssignmentDTOs
{
    public class PurchaseDeliveryForDetailDTO
    {
        public PurchaseDeliveryForDetailDTO(DateTime date, string street, string city, string postalCode, decimal totalPrice, PriorityType priority )
        {
            Date = date;
            Street = street;
            City = city;
            PostalCode = postalCode;
            TotalPrice = totalPrice;
            Priority = priority;
        }
        public DateTime Date { get; set; }
        public PriorityType Priority { get; set; }
        [Required, StringLength(60)]
        public string City { get; set; }

        [Required, StringLength(120)]
        public string Street { get; set; }

        [Required, StringLength(20)]
        public string PostalCode { get; set; }
        [Precision(10, 2)]
        [Range(typeof(decimal), "0.00", "9999999999.99")]
        public decimal TotalPrice { get; set; }

    }
}
