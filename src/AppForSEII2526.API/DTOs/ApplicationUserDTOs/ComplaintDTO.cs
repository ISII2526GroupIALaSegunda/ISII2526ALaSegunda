namespace AppForSEII2526.API.DTOs.ApplicationUserDTOs
{
    public class ComplaintDTO
    {
        public ComplaintDTO(string type, int count)
        {
            Type = type;
            Count = count;
        }

        public string Type { get; set; }
        public int Count {  get; set; }
    }
}
