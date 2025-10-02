namespace AppForSEII2526.API.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(60)]
        public string City { get; set; } = default!;

        [Required, StringLength(120)]
        public string Street { get; set; } = default!;

        [Required, StringLength(20)]
        public string PostalCode { get; set; } = default!;

        [Required, StringLength(120)]
        public string NameSurname { get; set; } = default!;

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime Date { get; set; }

        [Range(0, 5)]
        public int? Rating { get; set; }

        [Precision(10, 2)]
        [Range(typeof(decimal), "0.00", "9999999999.99")]
        public decimal TotalPrice { get; set; }

        public PurchaseState PurchaseState { get; set; }
        public IList<PurchaseProduct> PurchaseProducts { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PurchaseState State { get; set; }
        public ApplicationUser Customer { get; set; }

    }
    public enum PurchaseState
    {
        Request,
        Processing,
        Delivery,
        Done
    }

}
