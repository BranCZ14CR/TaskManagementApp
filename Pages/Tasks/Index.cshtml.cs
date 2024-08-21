using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Pages.Tasks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        //Atributos de la clase
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        //Constructor de la clase
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

        //Variables para tener la lista de tareas, la paginacion de la tabla y una bandera para ver las que estan completas o no
        public IList<TaskItemViewModel> TaskItems { get; set; } = new List<TaskItemViewModel>();
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool ShowCompleted { get; set; }

        //Metodo para cargar datos antes de cargar la pagina
        public async Task OnGetAsync(int pageNumber = 1, bool showCompleted = false)
        {
            var pageSize = 10; // Número de tareas que se mostrarán por página
            PageNumber = pageNumber; // Número de la página actual
            ShowCompleted = showCompleted; // Indica si se deben mostrar las tareas completadas

            // Obtener el usuario actual
            var currentUser = await _userManager.GetUserAsync(User);

            // Verificar que el usuario no sea nulo antes de proceder
            if (currentUser != null)
            {
                // Verificar si el usuario actual es un superusuario (administrador)
                var isSuperUser = await _userManager.IsInRoleAsync(currentUser, "SuperUser");

                // Crear una consulta para obtener las tareas
                var query = _context.TaskItems.AsQueryable();

                // Si el usuario no es superusuario, filtrar las tareas para mostrar solo las asignadas a este usuario
                if (!isSuperUser)
                {
                    query = query.Where(t => t.UsuarioId == currentUser.Id);
                }

                // Si no se deben mostrar tareas completadas, filtrar las tareas para mostrar solo las no completadas
                if (!showCompleted)
                {
                    query = query.Where(t => !t.Completada);
                }

                // Contar el número total de tareas que cumplen con los filtros aplicados
                var totalTasks = await query.CountAsync();

                // Calcular el número total de páginas necesarias para la paginación
                TotalPages = (int)Math.Ceiling(totalTasks / (double)pageSize);

                // Obtener las tareas para la página actual con paginación
                var tasks = await query
                    .OrderBy(t => t.FechaVencimiento)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Asignar el correo electrónico a cada tarea en el ViewModel para su visualización
                foreach (var task in tasks)
                {
                    var user = await _userManager.FindByIdAsync(task.UsuarioId);
                    var viewModel = new TaskItemViewModel
                    {
                        TaskItem = task,
                        UsuarioEmail = user?.Email
                    };
                    TaskItems.Add(viewModel);
                }

                // Guardar el estado de mostrar tareas completadas en el ViewData para usarlo en la vista
                ViewData["ShowCompleted"] = showCompleted;
            }
        }

        //Manejador para cambiar el estado de completado de una tarea
        public async Task<IActionResult> OnPostToggleCompletedAsync(int id)
        {
            // Buscar la tarea por su ID
            var task = await _context.TaskItems.FindAsync(id);

            // Verificar que la tarea exista
            if (task == null)
            {
                return new JsonResult(new { success = false, message = "Tarea no encontrada" });
            }

            // Cambiar el estado de completada de la tarea
            task.Completada = !task.Completada;

            // Actualizar la tarea en la base de datos
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();

            // Devolver una respuesta en formato JSON indicando el éxito y el nuevo estado de la tarea
            return new JsonResult(new { success = true, completada = task.Completada });
        }
    }
}
