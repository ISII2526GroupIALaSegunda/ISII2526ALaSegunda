namespace AppForSEII2526.API.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class Product
    {
        public string Colour { get; set; }

        public string Description { get; set; }

        public bool IsReturnable { get; set; }

        public string Name { get; set; }

        [Precision(5, 2)]
        public decimal Price { get; set; }

        public int ProductId { get; set; }

        public int Stock { get; set; }

        public Brand Brand { get; set; }

        public IList<PurchaseProduct> PurchaseProducts { get; set; }

    }
}
