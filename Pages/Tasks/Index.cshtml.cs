using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        // ViewModel para pasar los datos a la vista
        public class TaskItemViewModel
        {
            public TaskItem TaskItem { get; set; }
            public string UsuarioEmail { get; set; }
        }

        public IList<TaskItemViewModel> TaskItems { get; set; } = new List<TaskItemViewModel>();

        public async Task OnGetAsync(bool showCompleted = false)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var isSuperUser = await _userManager.IsInRoleAsync(currentUser, "SuperUser");

                var query = _context.TaskItems.AsQueryable();

                if (!isSuperUser)
                {
                    query = query.Where(t => t.UsuarioId == currentUser.Id);
                }

                if (!showCompleted)
                {
                    query = query.Where(t => !t.Completada);
                }

                var tasks = await query
                    .OrderBy(t => t.FechaVencimiento)
                    .ToListAsync();

                // Asignar el correo electrónico a cada tarea en el ViewModel
                foreach (var task in tasks)
                {
                    var user = await _userManager.FindByIdAsync(task.UsuarioId);
                    var email = user?.Email ?? "No asignado";

                    TaskItems.Add(new TaskItemViewModel
                    {
                        TaskItem = task,
                        UsuarioEmail = email
                    });
                }
            }
            else
            {
                // Manejo de error: Usuario no encontrado
                TaskItems = new List<TaskItemViewModel>();
            }

            ViewData["ShowCompleted"] = showCompleted;
        }
    }
}
