using Core.DAL.Tareas;
using Core.Model.Dto;
using Core.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Core.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ILogger<TareasController> log;
        private readonly ITareaRepository tareaRepository;

        public TareasController(ILogger<TareasController> logger, ITareaRepository tareaSender)
        {
            log = logger;
            tareaRepository = tareaSender;            
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodos()
        {
            log.LogDebug("Obtener todas las tareas");
            try
            {
                List<TareaResponse> data = await tareaRepository.Select(p => new TareaResponse
                {
                    Id = p.Id,
                    Accion = p.Accion,
                    Completada = p.Completada ,
                    Fechahoradeactualizacion=p.Fechahoradeactualizacion,
                    FechaHoradecreacion = p.FechaHoradecreacion
                }).ToListAsync<TareaResponse>();
               return Ok(data);
                
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensaje = ex.Message.ToString() });
            }           
        }

        [HttpGet("id={id}")]
        public async Task<ActionResult> ObterTarea(int id)
        {
            log.LogDebug("Obtener tarea id:" + id.ToString());
            try
            {
                Tarea data = await tareaRepository.GetAsync(id);
                return Ok(data);

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensaje = ex.Message.ToString() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> NuevoRegistroTarea([FromBody] TareaRequest itemTareadto)
        {
            log.LogDebug("Crear una nueva tarea: " + itemTareadto.Accion);
            try
            {
                Tarea nuevoRegistro= new ()
                {
                    Accion = itemTareadto.Accion,
                    Completada = false,
                    FechaHoradecreacion = DateTime.Now,
                    Id=0
                };

                await tareaRepository.AddAsync(nuevoRegistro);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensaje = ex.Message.ToString() });
            }
        }

        [HttpPut]
        public async Task<IActionResult> ModificaRegistroTarea([FromBody] TareaResponse itemTareadto)
        {
            log.LogDebug("Modifica una tarea: " + itemTareadto.Id);
            try
            {
                Tarea existenteRegistro = await tareaRepository.GetAsync(itemTareadto.Id);
                if (existenteRegistro == null)
                {
                    return BadRequest(new ErrorResponse() { 
                        Id=1,
                        Mensaje = "No existe la tarea con el identificador enviado" });
                }
                else
                {
                    existenteRegistro.Accion = itemTareadto.Accion;
                    existenteRegistro.Completada = itemTareadto.Completada;
                   
                    await tareaRepository.UpdateAsync(existenteRegistro);
                    return Ok(existenteRegistro);
                }                                                  
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensaje = ex.Message.ToString() });
            }
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> EliminaRegistroTarea(int id)
        {
            log.LogDebug("Eliminar tarea id: " + id);
            try
            {                            
                await tareaRepository.DeleteAsync(id);
                return Ok();
                
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensaje = ex.Message.ToString() });
            }
        }
    }
}
