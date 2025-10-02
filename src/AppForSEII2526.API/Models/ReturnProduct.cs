namespace AppForSEII2526.API.Models
{
    public class ReturnProduct
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for returning is 1")]
        public int Quantity { get; set; }

        [StringLength(100, ErrorMessage = "Reason cannot be longer than 100 characters",
        MinimumLength = 1)]
        public string Reason { get; set; }

        public ReturnPurchaseOrder ReturnPurchaseOrder { get; set; }

        public PurchaseProduct PurchaseProduct { get; set; }
        public int ProductId { get; set; }
        public int PurchaseOrderId { get; set; }
    }
}
