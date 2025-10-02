using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;


public class ApplicationUser : IdentityUser {
    [PersonalData]
    [StringLength(50)]
    public string? Name { get; set; }

    [PersonalData]
    [StringLength(50)]
    public string? Surname { get; set; }

    [PersonalData]
    [StringLength(200)]
    public string? Address { get; set; }

    public DateTime? AccountCreationDate { get; set; }
    public IList<PurchaseOrder>? PurchaseOrders { get; set; }



}