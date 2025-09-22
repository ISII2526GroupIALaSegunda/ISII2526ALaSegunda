namespace AppForSEII2526.API.Models
{
    public class PurchaseProduct
    {
        [Key]
        public int Id { get; set; }  

        public int PurchaseOrderId { get; set; }
   
        public int ProductId { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must provide a quantity higher than 1")]
        public int Quantity { get; set; }

    

    }
}
