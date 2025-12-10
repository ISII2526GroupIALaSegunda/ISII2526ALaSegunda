namespace AppForSEII2526.API.DTOs.BanUserDTOs
{
    public class ReportCustomerForDetailDTO
    {
        public ReportCustomerForDetailDTO(int customerId, string name, string surname, string? personalMessage)
        {
            CustomerId = customerId;
            Name = name;
            Surname = surname;
            PersonalMessage = personalMessage;
        }

        public int CustomerId { get; set; }

        [Required, StringLength(60, ErrorMessage = "Name cannot be longer than 60 characters.")]
        public string Name { get; set; }

        [Required, StringLength(60, ErrorMessage = "Surname cannot be longer than 60 characters.")]
        public string Surname { get; set; }

        [StringLength(250, ErrorMessage = "Personal message cannot be longer than 250 characters.")]
        public string? PersonalMessage { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReportCustomerForDetailDTO dto &&
                   CustomerId == dto.CustomerId &&
                   Name == dto.Name &&
                   Surname == dto.Surname &&
                   PersonalMessage == dto.PersonalMessage;
        }
    }
}
