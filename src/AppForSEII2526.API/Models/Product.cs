namespace AppForSEII2526.API.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = default!;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(30)]
        public string? Colour { get; set; }

        [Precision(10, 2)]
        [Range(typeof(decimal), "0.01", "9999999999.99",
               ErrorMessage = "Price must be at least 0.01")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public bool IsReturnable { get; set; }  

    }
