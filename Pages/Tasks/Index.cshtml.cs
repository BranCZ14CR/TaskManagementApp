using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementApp.Pages.Tasks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<TaskItem> TaskItem { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var isSuperUser = await _userManager.IsInRoleAsync(currentUser, "SuperUser");
                if (isSuperUser)
                {
                    TaskItem = await _context.TaskItems.ToListAsync();
                }
                else
                {
                    TaskItem = await _context.TaskItems
                        .Where(t => t.UsuarioId == currentUser.Id)
                        .ToListAsync();
                }
            }
            else
            {
                // Manejo de error: Usuario no encontrado
                TaskItem = new List<TaskItem>();
            }
        }
    }
}
