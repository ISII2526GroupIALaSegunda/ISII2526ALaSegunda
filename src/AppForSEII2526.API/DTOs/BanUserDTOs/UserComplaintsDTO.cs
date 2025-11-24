namespace AppForSEII2526.API.DTOs.BanUserDTOs
{
    public class UserComplaintsDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public int ComplaintCount { get; set; }
        public List<string> ComplaintTypes { get; set; }
    }

    // DTO para aplicar filtros
    public class ComplaintFilterDTO
    {
        public string? Surname { get; set; }
        public string? ComplaintType { get; set; }
    }

    // DTO de respuesta genérica
    public class ComplaintsResponseDTO
    {
        public bool HasComplaints { get; set; }
        public List<UserComplaintsDTO> Users { get; set; } = new();
        public string? Message { get; set; }

    }
}
