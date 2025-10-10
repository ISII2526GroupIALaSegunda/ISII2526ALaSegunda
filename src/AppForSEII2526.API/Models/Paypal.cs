namespace AppForSEII2526.API.Models
{
    public class PayPal : PaymentMethod
    {
        [EmailAddress]
        [StringLength(254, ErrorMessage = "Email should not be longer than 254 characters")]
        public string Email { get; set; }

    }
}
