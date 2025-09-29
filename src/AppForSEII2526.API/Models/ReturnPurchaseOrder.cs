namespace AppForSEII2526.API.Models
{        
    
    [Index(nameof(Name), IsUnique = true)]
    public class ReturnPurchaseOrder
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public double MoneyToReturn { get; set; }

       [Precision(5,2)]
        public decimal NewTotalPrice { get; set; }

        public int? Rating { get; set; }

        [Precision(5, 2)]
        public decimal TotalPrice { get; set; }
    }

   
}
