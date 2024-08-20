using System;
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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; } = default!;

        public string UserEmail { get; set; }

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

            var user = await _userManager.FindByIdAsync(taskitem.UsuarioId);
            if (user == null)
            {
                return NotFound();
            }

            TaskItem = taskitem;
            UserEmail = user.UserName;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskitem = await _context.TaskItems.FindAsync(id);
            if (taskitem != null)
            {
                TaskItem = taskitem;
                _context.TaskItems.Remove(TaskItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
