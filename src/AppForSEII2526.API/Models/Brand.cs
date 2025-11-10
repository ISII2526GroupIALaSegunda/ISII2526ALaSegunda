namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Brand
    {
        public Brand()
        {
        }

        public Brand(int id, string location, string name)
        {
            Id = id;
            Location = location;
            Name = name;
        }

        public int Id { get; set; }

        public string Location { get; set; }
        [Required, StringLength(80)]
        public string Name { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
