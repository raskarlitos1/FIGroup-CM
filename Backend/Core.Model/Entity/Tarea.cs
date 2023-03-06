using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Model.Entity
{
    /// <summary>
    /// Modelo de un registro en la entidad Tarea
    /// </summary>
    public class Tarea
    {
        /// <summary>
        /// Identificador único de la tarea en la base de datos
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Acción de la tarea
        /// </summary>
        [Required]
        [StringLength(128)]
        public string Accion { get; set; }

        /// <summary>
        /// Identifica el estado de la tarea; true= Completada, false=No completada
        /// </summary>
        [Required]
        public bool Completada { get; set; }

        /// <summary>
        /// Identifica la fecha y hora en la que se creó el registro
        /// </summary>
        [Required]
        public DateTime FechaHoradecreacion { get; set; }

        /// <summary>
        /// Identifica la fecha y hora en la que se actualizó el registro por última vez
        /// </summary>
        public DateTime? Fechahoradeactualizacion { get; set; }
    }
}
