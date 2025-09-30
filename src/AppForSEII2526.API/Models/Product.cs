namespace AppForSEII2526.API.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Title { get; set; } = default!;

        [Precision(10, 2)]
        [Range(typeof(decimal), "0.10", "1000000", ErrorMessage = "Price must be at least 0.10")]
        public decimal Price { get; set; }

        public int Stock { get; set; }


    }
}
