namespace AppForSEII2526.API.Models
{
    public class CreditCard : PaymentMethod
    {
        [Required]
        [CreditCard]
        [RegularExpression(@"^\d{13,19}$", ErrorMessage = "Credit card number must contain 13 to 19 digits.")]
        [StringLength(19, ErrorMessage = "Credit card number cannot exceed 19 digits.")]
        public string CreditCardNumber { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime ExpirationDate { get; set; }

    }
}
