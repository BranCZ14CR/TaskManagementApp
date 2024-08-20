using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Pages.Tasks
{
    [Authorize] //Controlar que solo usuarios autenticados puedan ver esta pantalla
    public class CreateModel : PageModel
    {
        //Atributos de la clase
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        //Constructor de la clase
        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Variable del Modelo para poder manipular sus atributos
        [BindProperty]
        public TaskItem TaskItem { get; set; } = default!;

        //Metodo antes de mostrar la pantalla
        public async Task<IActionResult> OnGetAsync()
        {
            //Obtener el usuario actual de la sesion
            var currentUser = await _userManager.GetUserAsync(User);
            
            //Si no se encuentra o es nulo mostrar la pantalla 404
            if (currentUser == null)
            {
                return NotFound();
            }

            /*
                Comprobar si es el administrador para que pueda asignar tareas a todas las cuentas, en caso de que no sea
                el administrador solo puede asignarse tareas a si mismo
            */
            if (await _userManager.IsInRoleAsync(currentUser, "SuperUser"))
            {
                // Si es SuperUser, muestra todos los usuarios
                ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email");
            }
            else
            {
                // Si no es SuperUser, muestra solo su propio usuario
                ViewData["UsuarioId"] = new SelectList(_context.Users
                    .Where(u => u.Id == currentUser.Id), "Id", "Email");
            }

            return Page();
        }

        //Metodo para crear la tarea
        public async Task<IActionResult> OnPostAsync()
        {
            //Verificar si el formulario es incorrecto retornar a la pantalla
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            //En caso de que el formulario este bien, guarda en base de datos y redirecciona a la pantalla principal
            _context.TaskItems.Add(TaskItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}