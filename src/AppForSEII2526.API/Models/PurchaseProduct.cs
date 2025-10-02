namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(PurchaseOrderId),nameof(ProductId))]
    public class PurchaseProduct
    {
       
        public int PurchaseOrderId { get; set; }
   
        [Precision(10, 2)]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must provide a quantity higher than 1")]
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }



        public ReturnProduct ReturnProduct { get; set; }
    }
}
