namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ComplaintType
    {
        public int ID { get; set; }

        [StringLength(20, ErrorMessage = "Name should not be longer than 20 characters")]
        public string Name { get; set; }
        

    }
}
