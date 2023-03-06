namespace Core.Model.Dto
{
    public class ErrorResponse
    {
        /// <summary>
        /// Identificador único de la tarea en la entidad
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mensaje de error
        /// </summary>                
        public string Mensaje { get; set; }
    }
}
