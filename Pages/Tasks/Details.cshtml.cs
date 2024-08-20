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
    [Authorize] //Control para que alguien no autenticado pueda entrar a ver la pantalla
    public class DetailsModel : PageModel
    {
        //Atributos de la clase
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        //Constructor de la clase
        public DetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Instanciar objeto del Modelo de las Tareas
        public TaskItem TaskItem { get; set; } = default!;
        public string UserEmail { get; set; } //Variable para obtener el correo del usuario

        //Metodo que carga la informacion antes de mostrar la pantalla
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //Verificar si el id de la tarea es nulo, muestra la pantalla 404
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            //Encontrar la primera tarea con el id del parametro
            var taskitem = await _context.TaskItems.FirstOrDefaultAsync(m => m.Id == id);

            //Comprobar si no existe en BD la tarea con ese ID
            if (taskitem == null)
            {
                return RedirectToPage("/NotFound");
            }

            //Obtener el usuario que tiene esa tarea asignada
            var user = await _userManager.FindByIdAsync(taskitem.UsuarioId);

            //Si no existe el usuario, muestra la pantalla 404
            if (user == null)
            {
                return RedirectToPage("/NotFound");
            }

            //Obtenener la informacion para mostrar en la pantalla
            TaskItem = taskitem;
            UserEmail = user.UserName!;

            return Page();
        }
    }
}
