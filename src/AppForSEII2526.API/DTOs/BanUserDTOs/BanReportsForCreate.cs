namespace AppForSEII2526.API.DTOs.BanUserDTOs
{
    public class BanReportsForCreate
    {
        [Required, StringLength(200)]
        public string Reason { get; set; }

        [Required, StringLength(500)]
        public string DetailedDescription { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        //[Required]
       // public List<ReportCustomerForCreationDTO> ReportedUsers { get; set; } = new();
    }
}
