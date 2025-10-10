namespace AppForSEII2526.API.Models
{
    public class Bizum : PaymentMethod
    {
        [Required]
        [Phone]
        [StringLength(20, ErrorMessage = "Telephone number should not be longer than 20 characters")]
        public string TelephoneNumber { get; set; }

    }
}
