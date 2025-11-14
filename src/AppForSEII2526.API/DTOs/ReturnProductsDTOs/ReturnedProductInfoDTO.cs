namespace AppForSEII2526.API.DTOs.ReturnProductsDTOs
{

}
namespace AppForSEII2526.API.DTOs.ReturnProductsDTOs
{
    public class ReturnedProductInfoDTO
    {
        public ReturnedProductInfoDTO(
            int quantity,
            
            string productName,
            string brandName,
            string warehouseLocation
            
        )
        {
            ProductName = productName;
            BrandName = brandName;
            WarehouseLocation = warehouseLocation;
            Quantity = quantity;
        }

        public int PurchaseProductId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string WarehouseLocation { get; set; }
        public int Quantity { get; set; }
    }
}
