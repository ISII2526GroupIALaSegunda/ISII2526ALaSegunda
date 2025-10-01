namespace AppForSEII2526.API.Models
{
    public class CreditCard : PaymentMethod
    {
        public string CreditCardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
