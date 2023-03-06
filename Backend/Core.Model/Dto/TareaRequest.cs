using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model.Dto
{
    public class TareaRequest
    {
        /// <summary>
        /// Acción de la tarea
        /// </summary>
        [StringLength(128)]
        public string Accion { get; set; }        
       
    }
}
