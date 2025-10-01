namespace AppForSEII2526.API.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = default!;

        public string? Location { get; set; }
    }
}
