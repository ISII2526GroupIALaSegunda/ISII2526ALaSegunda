namespace AppForSEII2526.API.Models
{
    public class PurchaseProduct
    {
        [Precision(5, 2)]
        public decimal Price { get; set; }

        public int ProductId { get; set; }

        public int PurchaseOrderId { get; set; }

        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
