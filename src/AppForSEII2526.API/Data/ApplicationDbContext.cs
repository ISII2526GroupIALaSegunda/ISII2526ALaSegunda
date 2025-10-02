using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<ReturnPurchaseOrder> Returns { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ReturnProduct> ReturnProducts { get; set; }

    public DbSet<DeliveryAssignment> DeliveryAssignments { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseProduct> PurchaseProducts { get; set; }
    public DbSet<PurchaseDelivery> PurchaseDeliveries { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<PayPal> PayPals { get; set; }
    public DbSet<Bizum> Bizums { get; set; }
    public DbSet<DeliveryDriver> DeliveryDrivers { get; set; }
    public DbSet<BanReport> BanReports { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<Complaint> Complaints { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers{ get; set;}
    public DbSet<ComplaintType> ComplaintTypes { get; set; }
}
