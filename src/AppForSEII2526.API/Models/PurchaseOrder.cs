namespace AppForSEII2526.API.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public string? CustomerUserId { get; set; }

        [Required, StringLength(120)]
        public string NameSurname { get; set; } = default!;

        [Required, StringLength(60)]
        public string City { get; set; } = default!;

        public string Street { get; set; } = default!;

        public string PostCode { get; set; } = default!;

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime Date { get; set; }

        [Range(0, 3)]
        public int State { get; set; }

        [Precision(10, 2)]
        
        public decimal TotalPrice { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; }

        public PurchaseDelivery DriverAssigned { get; set; }
        public List<PurchaseProduct> Products { get; set; }

    }

}
