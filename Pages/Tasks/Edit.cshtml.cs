using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Pages.Tasks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly TaskManagementApp.Data.ApplicationDbContext _context;

        public EditModel(TaskManagementApp.Data.ApplicationDbContext context)
        {
            _context = context;
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

            // Load the list of users for the dropdown
            Usuarios = new SelectList(await _context.Users.ToListAsync(), "Id", "Email");

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
                // Reload the list of users in case of a validation error
                Usuarios = new SelectList(await _context.Users.ToListAsync(), "Id", "Email", TaskItem.UsuarioId);
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
