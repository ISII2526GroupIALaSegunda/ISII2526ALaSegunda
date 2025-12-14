using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Data;

public static class SeedData
{
    public static async Task Initialize(ApplicationDbContext context, IServiceProvider serviceProvider, ILogger logger)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync("Client"))
        {
            await roleManager.CreateAsync(new IdentityRole("Client"));
            logger.LogInformation("Rol 'Client' creado.");
        }

        var userEmail = "pepe@uclm.es";
        var existingUser = await userManager.FindByEmailAsync(userEmail);

        ApplicationUser user;
        if (existingUser == null)
        {
            user = new ApplicationUser
            {
                Id = "1",
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true,
                Name = "Pepe",
                Surname = "Perez",
                Address = "Calle Inventada, 1",
                AccountCreationDate = new DateTime(2020, 12, 6),
                PhoneNumber = "+34611343434",
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Password1234%");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Client");
                logger.LogInformation("Usuario '{Email}' creado con contraseña 'Password1234%' y rol 'Client'.", userEmail);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    logger.LogError("Error creando usuario: {Error}", error.Description);
                }
                return; // No continuar si el usuario no se creó
            }
        }
        else
        {
            user = existingUser;
            logger.LogInformation("Usuario '{Email}' ya existe.", userEmail);
        }

        Brand? brandZara = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Zara");
        if (brandZara == null)
        {
            brandZara = new Brand { Location = "Madrid", Name = "Zara" };
            context.Brands.Add(brandZara);
            await context.SaveChangesAsync();
            logger.LogInformation("Brand 'Zara' creada con Id={Id}.", brandZara.Id);
        }

        Brand? brandUniqlo = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Uniqlo");
        if (brandUniqlo == null)
        {
            brandUniqlo = new Brand { Location = "Barcelona", Name = "Uniqlo" };
            context.Brands.Add(brandUniqlo);
            await context.SaveChangesAsync();
            logger.LogInformation("Brand 'Uniqlo' creada con Id={Id}.", brandUniqlo.Id);
        }

        Product? productJacket = await context.Products.FirstOrDefaultAsync(p => p.Name == "Jacket");
        if (productJacket == null)
        {
            productJacket = new Product
            {
                Name = "Jacket",
                Description = null,
                Colour = "Red",
                Price = 20.00m,
                Stock = 4,
                IsReturnable = true,
                Brand = brandZara
            };
            context.Products.Add(productJacket);
            await context.SaveChangesAsync();
            logger.LogInformation("Product 'Jacket' creado con Id={Id}.", productJacket.ProductId);
        }

        Product? productShirt = await context.Products.FirstOrDefaultAsync(p => p.Name == "Shirt");
        if (productShirt == null)
        {
            productShirt = new Product
            {
                Name = "Shirt",
                Description = null,
                Colour = "Blue",
                Price = 10.00m,
                Stock = 2,
                IsReturnable = false,
                Brand = brandUniqlo
            };
            context.Products.Add(productShirt);
            await context.SaveChangesAsync();
            logger.LogInformation("Product 'Shirt' creado con Id={Id}.", productShirt.ProductId);
        }

        Bizum? bizum = await context.PaymentMethods.OfType<Bizum>()
            .FirstOrDefaultAsync(pm => pm.TelephoneNumber == "+34611343434");
        if (bizum == null)
        {
            bizum = new Bizum
            {
                User = user,
                TelephoneNumber = "+34611343434"
            };
            context.PaymentMethods.Add(bizum);
            await context.SaveChangesAsync();
            logger.LogInformation("PaymentMethod Bizum creado con Id={Id}.", bizum.Id);
        }

        PurchaseOrder? purchaseOrder = await context.PurchaseOrders
            .FirstOrDefaultAsync(po => po.NameSurname == "Pepito" && po.City == "Albacete");
        if (purchaseOrder == null)
        {
            purchaseOrder = new PurchaseOrder
            {
                City = "Albacete",
                Street = "Calle Inventada",
                PostalCode = "02001",
                NameSurname = "Pepito",
                Description = null,
                Date = new DateTime(2025, 12, 12),
                Rating = null,
                TotalPrice = 30.00m,
                State = PurchaseState.Processing,
                Customer = user,
                PaymentMethod = bizum
            };
            context.PurchaseOrders.Add(purchaseOrder);
            await context.SaveChangesAsync();
            logger.LogInformation("PurchaseOrder creada con Id={Id}.", purchaseOrder.Id);
        }

        if (!await context.PurchaseProducts.AnyAsync(pp => pp.PurchaseOrderId == purchaseOrder.Id && pp.ProductId == productJacket.ProductId))
        {
            context.PurchaseProducts.Add(new PurchaseProduct
            {
                PurchaseOrderId = purchaseOrder.Id,
                ProductId = productJacket.ProductId,
                Price = 20.00m,
                Quantity = 1,
                PurchaseOrder = purchaseOrder,
                Product = productJacket
            });
            await context.SaveChangesAsync();
            logger.LogInformation("PurchaseProduct (Order={OrderId}, Product={ProductId}) creado.", purchaseOrder.Id, productJacket.ProductId);
        }

        if (!await context.PurchaseProducts.AnyAsync(pp => pp.PurchaseOrderId == purchaseOrder.Id && pp.ProductId == productShirt.ProductId))
        {
            context.PurchaseProducts.Add(new PurchaseProduct
            {
                PurchaseOrderId = purchaseOrder.Id,
                ProductId = productShirt.ProductId,
                Price = 10.00m,
                Quantity = 1,
                PurchaseOrder = purchaseOrder,
                Product = productShirt
            });
            await context.SaveChangesAsync();
            logger.LogInformation("PurchaseProduct (Order={OrderId}, Product={ProductId}) creado.", purchaseOrder.Id, productShirt.ProductId);
        }

        logger.LogInformation("Seed data completado.");
    }
}
