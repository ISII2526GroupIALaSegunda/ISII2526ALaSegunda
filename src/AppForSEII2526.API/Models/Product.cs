namespace AppForSEII2526.API.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class Product
    {
        public Product()
        {
        }

        public Product(int productId, string name, string? colour, decimal price, int stock, bool isReturnable, Brand brand)
        {
            ProductId = productId;
            Name = name;
            Colour = colour;
            Price = price;
            Stock = stock;
            IsReturnable = isReturnable;
            Brand = brand;
        }

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
        public Brand Brand { get; set; }

        public IList<PurchaseProduct> PurchaseProducts { get; set; }


    }
}
