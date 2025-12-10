
namespace AppForSEII2526.API.DTOs_PurchaseProductDTOs
{
    public class PurchaseProductsForReturningDTO
    {
        public PurchaseProductsForReturningDTO(int id, string productName, int quantity, string branchName, string branchLocation, bool isReturnable)
        {
            Id = id;
            Productname = productName;
            Quantity = quantity;
            Branchname = branchName;
            Branchlocation = branchLocation;
            IsReturnable = isReturnable;
        }

        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage = "Product name cannot be longer than 50 characters.")]
        public string Productname { get; set; }
        public int Quantity { get; set; }
        
        [StringLength(50, ErrorMessage = "Branch name cannot be longer than 50 characters.")]
        public string Branchname { get; set; }
        
        public string Branchlocation { get; set; }
        
        public bool IsReturnable { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseProductsForReturningDTO dTO &&
                   Id == dTO.Id &&
                   Productname == dTO.Productname &&
                   Quantity == dTO.Quantity &&
                   Branchname == dTO.Branchname &&
                   Branchlocation == dTO.Branchlocation &&
                   IsReturnable == dTO.IsReturnable;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Productname, Quantity, Branchname, Branchlocation, IsReturnable);
        }
    }


}
