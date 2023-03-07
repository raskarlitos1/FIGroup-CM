using System.ComponentModel.DataAnnotations;

namespace Core.Model.Dto
{
    public class TareaResponse
    {
        /// <summary>
        /// Identificador único de la tarea en la entidad
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Acción de la tarea
        /// </summary>        
        [StringLength(128)]
        public string Accion { get; set; }

        /// <summary>
        /// Identifica el estado de la tarea; true= Completada, false=No completada
        /// </summary>        
        public bool Completada { get; set; }

        /// <summary>
        /// Identifica la fecha y hora en la que se creó el registro
        /// </summary>        
        public DateTime FechaHoradecreacion { get; set; }

        /// <summary>
        /// Identifica la fecha y hora en la que se actualizó el registro por última vez
        /// </summary>
        public DateTime? Fechahoradeactualizacion { get; set; }      

    }
}
