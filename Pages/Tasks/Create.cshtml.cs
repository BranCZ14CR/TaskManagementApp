using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Pages.Tasks
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly TaskManagementApp.Data.ApplicationDbContext _context;

        public CreateModel(TaskManagementApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            TaskItem = new TaskItem();
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; } = default!;

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
