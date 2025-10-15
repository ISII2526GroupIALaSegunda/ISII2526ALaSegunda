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

        public int ProductId { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = default!;
        [StringLength(30)]
        public string? Colour { get; set; }

        public string Brand { get; set; }




    }
}
