
namespace AppForSEII2526.API.DTOs.ApplicationUserDTOs
{
    public class ApplicationUserForBanningDTO
    {
       

        public ApplicationUserForBanningDTO(string id, string userName, string surname, DateTime accountCreationDate, IList<ComplaintDTO> complaints)
        {
            Id = id;
            UserName = userName;
            Surname = surname;
            CreationDate = accountCreationDate;
            Complaints = complaints;
            
        }

        public string Id { get; set; }

        [StringLength(50, ErrorMessage = "User name must not exceed 50 characters")]
        public string UserName { get; set; }

        [StringLength(50, ErrorMessage = "Surname must not exceed 50 characters")]
        public string Surname { get; set; }


        public IList<ComplaintDTO> Complaints { get; set; }
        public DateTime CreationDate { get; set; }

       
    }
}
