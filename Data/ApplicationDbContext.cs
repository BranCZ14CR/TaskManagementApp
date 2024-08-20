using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Models;

namespace TaskManagementApp.Data
{
    /*
        Clase principal para usar la conexion a la base de datos
     */
    public class ApplicationDbContext : IdentityDbContext
    {
        //Constructor de la clase
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Agregar Tablas a la Base de Datos

        //Tabla de las tareas
        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
