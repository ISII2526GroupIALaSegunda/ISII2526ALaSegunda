namespace AppForSEII2526.API.Models
{
    public class ReturnProduct
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public string Reason { get; set; }

        public ReturnPurchaseOrder ReturnPurchaseOrder { get; set; }

        public PurchaseProduct PurchaseProduct { get; set; }
        public object ProductId { get; internal set; }
        public object PurchaseOrderId { get; internal set; }
    }
}
