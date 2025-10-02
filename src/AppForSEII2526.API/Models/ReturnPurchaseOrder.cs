namespace AppForSEII2526.API.Models
{        
    
    [Index(nameof(Name), IsUnique = true)]
    public class ReturnPurchaseOrder
    {
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "Name can be neither longer than 20 characters nor shorter than 1", 
        MinimumLength =1)]
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public double MoneyToReturn { get; set; }

       [Precision(5,2)]
        public decimal NewTotalPrice { get; set; }

        public int? Rating { get; set; }

        [Precision(5, 2)]
        public decimal TotalPrice { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }

        public IList<ReturnProduct> ReturnProducts { get; set; }

        public ApplicationUser Customer { get; set; }
    }


   
}
