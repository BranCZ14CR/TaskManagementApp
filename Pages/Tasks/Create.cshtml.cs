using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Pages.Tasks
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("Usuario no encontrado.");
            }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TaskItems.Add(TaskItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
