using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Pages.Tasks
{
    [Authorize] //Controlar que solo usuarios autenticados puedan ver esta pantalla
    public class EditModel : PageModel
    {
        //Atributos de la clase
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        //Constructor de la clase
        public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        //Variable del Modelo para poder manipular sus atributos
        public TaskItem TaskItem { get; set; } = default!;
        public SelectList Usuarios { get; set; } = default!; //Variable para obtener la lista de todos los usuarios del sistema
        public string UsuarioEmail { get; set; } = default!; //Variable para obtener el correo del usuario

        //Metodo que carga la informacion antes de mostrar la pantalla
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //Verificar si el id de la tarea es nulo, muestra la pantalla 404
            if (id == null)
            {
                return NotFound();
            }

            //Encontrar la primera tarea con el id del parametro
            var taskitem = await _context.TaskItems.FirstOrDefaultAsync(m => m.Id == id);

            //Comprobar si no existe en BD la tarea con ese ID
            if (taskitem == null)
            {
                return NotFound();
            }

            TaskItem = taskitem; //Agregar a la variable lo encontrado en la BD

            // Obtener el usuario actual
            var currentUser = await _userManager.GetUserAsync(User);

            //Si no existe el usuario, muestra la pantalla 404
            if (currentUser == null)
            {
                return NotFound();
            }

            /*
                Comprobar si el usuario es el administrador para mostrarle toda la lista de los usuarios, si no es asi entonces solo mostrar el usuario
                de la cuenta actual
             */
            if (await _userManager.IsInRoleAsync(currentUser, "SuperUser"))
            {
                // Si es SuperUser, muestra todos los usuarios
                Usuarios = new SelectList(await _context.Users.ToListAsync(), "Id", "Email", TaskItem.UsuarioId);
            }
            else
            {
                // Si no es SuperUser, muestra solo su propio usuario
                Usuarios = new SelectList(await _context.Users
                    .Where(u => u.Id == currentUser.Id).ToListAsync(), "Id", "Email", TaskItem.UsuarioId);
            }

            // Establece el correo electrónico del usuario actualmente asignado
            UsuarioEmail = await _context.Users
                .Where(u => u.Id == TaskItem.UsuarioId)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            return Page();
        }

        //Metodo para actualizar la tarea
        public async Task<IActionResult> OnPostAsync()
        {
            //Comprobar si el formulario es valido o no
            if (!ModelState.IsValid)
            {
                // Obtener el usuario actual
                var currentUser = await _userManager.GetUserAsync(User);

                /*
                    Comprobar si el usuario es el administrador para mostrarle toda la lista de los usuarios, si no es asi entonces solo mostrar el usuario
                    de la cuenta actual
                */
                if (await _userManager.IsInRoleAsync(currentUser!, "SuperUser"))
                {
                    // Si es SuperUser, muestra todos los usuarios
                    Usuarios = new SelectList(await _context.Users.ToListAsync(), "Id", "Email", TaskItem.UsuarioId);
                }
                else
                {
                    // Si no es SuperUser, muestra solo su propio usuario
                    Usuarios = new SelectList(await _context.Users
                        .Where(u => u.Id == currentUser!.Id).ToListAsync(), "Id", "Email", TaskItem.UsuarioId);
                }
                return Page();
            }

            _context.Attach(TaskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(TaskItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
