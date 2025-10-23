using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ProductDTOs
{
    public class ProductForPurchaseDTO
    {
        public ProductForPurchaseDTO(int productId, string name, string? colour, string brandName)
        {
            ProductId = productId;
            Name = name;
            Colour = colour;
            Brand = brandName;
        }

        public ProductForPurchaseDTO(int productId, string name, string? colour, string brandName, int stock, string? city)
        {
            ProductId = productId;
            Name = name;
            Colour = colour;
            Brand = brandName;
            Stock = stock;
            City = city;
        }

        public int ProductId { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = default!;
        [StringLength(30)]
        public string? Colour { get; set; }

        public string Brand { get; set; }

        // Stock from Product
        public int Stock { get; set; }

        // City from related PurchaseOrder (may be null if never purchased)
        public string? City { get; set; }
    }
}
