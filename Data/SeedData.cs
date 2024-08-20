using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Crear el rol de superusuario si no existe
        string superUserRole = "SuperUser";
        var roleExists = await roleManager.RoleExistsAsync(superUserRole);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(superUserRole));
        }

        // Crear el superusuario si no existe
        string superUserEmail = "admin@mail.com";
        var superUser = await userManager.FindByEmailAsync(superUserEmail);
        if (superUser == null)
        {
            superUser = new IdentityUser
            {
                UserName = superUserEmail,
                Email = superUserEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(superUser, "Qyvxdr58*");
        }

        // Asignar el rol de superusuario al superusuario
        if (!await userManager.IsInRoleAsync(superUser, superUserRole))
        {
            await userManager.AddToRoleAsync(superUser, superUserRole);
        }
    }
}
