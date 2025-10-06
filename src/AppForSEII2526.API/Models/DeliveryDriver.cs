namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class DeliveryDriver
    {
        public IList<DeliveryAssignment> Assignments { get; set; }
        public bool Available { get; set; }
        public int id { get; set; }
        [StringLength(20, ErrorMessage = "Name can not be longer than 200 characters")]
        public string Name { get; set; }
    }
}
