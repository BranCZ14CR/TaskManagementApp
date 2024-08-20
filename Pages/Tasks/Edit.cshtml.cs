using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Pages.Tasks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; } = default!;

        public SelectList Usuarios { get; set; } = default!;
        public string UsuarioEmail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskitem = await _context.TaskItems.FirstOrDefaultAsync(m => m.Id == id);
            if (taskitem == null)
            {
                return NotFound();
            }

            TaskItem = taskitem;

            // Obtener el usuario actual
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("Usuario no encontrado.");
            }

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

            // Set the email of the currently assigned user
            UsuarioEmail = await _context.Users
                .Where(u => u.Id == TaskItem.UsuarioId)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (await _userManager.IsInRoleAsync(currentUser, "SuperUser"))
                {
                    Usuarios = new SelectList(await _context.Users.ToListAsync(), "Id", "Email", TaskItem.UsuarioId);
                }
                else
                {
                    Usuarios = new SelectList(await _context.Users
                        .Where(u => u.Id == currentUser.Id).ToListAsync(), "Id", "Email", TaskItem.UsuarioId);
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
