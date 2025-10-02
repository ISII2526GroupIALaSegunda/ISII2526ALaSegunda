using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class

[Index(nameof(Name), IsUnique = true)]

public class ApplicationUser : IdentityUser
{

    public DateTime AccountCreationDate { get; set; }

    public string Address { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }
}