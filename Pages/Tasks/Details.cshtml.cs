using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TaskManagementApp.Data;
using TaskManagementApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagementApp.Pages.Tasks
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public TaskItem TaskItem { get; set; } = default!;
        public string UserEmail { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            var taskitem = await _context.TaskItems.FirstOrDefaultAsync(m => m.Id == id);
            if (taskitem == null)
            {
                return RedirectToPage("/NotFound");
            }

            var user = await _userManager.FindByIdAsync(taskitem.UsuarioId);
            if (user == null)
            {
                return RedirectToPage("/NotFound");
            }

            TaskItem = taskitem;
            UserEmail = user.UserName;

            return Page();
        }
    }
}
