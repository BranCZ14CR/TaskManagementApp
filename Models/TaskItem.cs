using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        // Título de la tarea
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres.")]
        [MinLength(10, ErrorMessage = "El título debe tener al menos 10 caracteres.")]
        public string Titulo { get; set; }

        // Descripción de la tarea
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres.")]
        public string Descripcion { get; set; }

        // Fecha límite para completar la tarea
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaVencimiento { get; set; }

        public bool Completada { get; set; } = false;

        public string UsuarioId { get; set; }
    }
}
