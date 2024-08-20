using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

/*
    Clase para crear al superusuario o al administrador, esto para que dicho usuario pueda ver todas las tareas de los demas usuarios
 */
public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        //Atributos para crear su rol y su cuenta
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Crear el rol de superusuario si no existe
        string superUserRole = "SuperUser";
        var roleExists = await roleManager.RoleExistsAsync(superUserRole);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(superUserRole));
        }

        // Crear la cuenta del superusuario si no existe
        string superUserEmail = "admin@mail.com"; //Correo del Admin
        var superUser = await userManager.FindByEmailAsync(superUserEmail);
        if (superUser == null)
        {
            superUser = new IdentityUser
            {
                UserName = superUserEmail,
                Email = superUserEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(superUser, "Qyvxdr58*"); //Constraseña del Admin
        }

        // Asignar el rol de superusuario al administrados
        if (!await userManager.IsInRoleAsync(superUser, superUserRole))
        {
            await userManager.AddToRoleAsync(superUser, superUserRole);
        }
    }
}
