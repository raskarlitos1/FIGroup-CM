using Core.Model;
using Core.Model.Dto;
using Core.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.DAL.Tareas
{
    /// <summary>
    /// Acceso a datos entidad Tarea
    /// </summary>
    public class TareaRepository : ITareaRepository
    {
        private readonly TareasDataContext dbContext;
        private readonly IQueryable<Tarea> dbSet;

        public TareaRepository(TareasDataContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<Tarea>();
        }
        public Expression Expression => dbSet.Expression;

        public Type ElementType => dbSet.ElementType;

        public IQueryProvider Provider => dbSet.Provider;

        public IEnumerator<Tarea> GetEnumerator() => dbSet.AsNoTracking().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Crea una nueva tarea
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        public async Task AddAsync(Tarea tarea)
        {
            if (tarea.Id.Equals(0))
            {
                dbContext.Entry(tarea).State = EntityState.Added;
                await dbContext.SaveChangesAsync();
            }                
            else
            {
                throw new Exception("El identificador de la tarea enviado no es válido");
            }
                
            
        }
        /// <summary>
        /// Modificar una tarea existente
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Tarea tarea)
        {
            tarea.Fechahoradeactualizacion = DateTime.Now;
            dbContext.Entry(tarea).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Eliminar una tarea existente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            Tarea tareaFinal = await GetAsync(id);
            dbContext.Entry(tareaFinal).State = EntityState.Deleted;
            await dbContext.SaveChangesAsync();
            dbContext.Entry(tareaFinal).State = EntityState.Detached;
        }

        /// <summary>
        /// Agregar varias tareas 
        /// </summary>
        /// <param name="tareas"></param>
        /// <returns></returns>
        public async Task<int> AddRangeAsync(IEnumerable<Tarea> tareas)
        {
            foreach (Tarea tarea in tareas)
            {
                dbContext.Entry(tareas).State = EntityState.Added;
            }

            return await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Busca una tarea por accion
        /// </summary>
        /// <param name="accion">acción</param>
        /// <returns></returns>
        public async Task<Tarea> GetAsync(string accion)
        {
            var resultado= await dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.Accion.Equals(accion));
            if (resultado == null)
                return null;
            else
                return new Tarea()
                {
                    Accion = resultado?.Accion ?? "",
                    Completada = resultado.Completada,
                    Fechahoradeactualizacion = resultado?.Fechahoradeactualizacion,
                    FechaHoradecreacion = resultado.FechaHoradecreacion,
                    Id = resultado.Id
                };
        }

        /// <summary>
        /// Busca una tarea por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Tarea> GetAsync(int id)
        {
            var resultado = await dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.Id.Equals(id));
            if (resultado != null)
            {                 
                return new Tarea()
                {
                    Accion = resultado.Accion ?? "",
                    Completada = resultado.Completada,
                    Fechahoradeactualizacion = resultado.Fechahoradeactualizacion,
                    FechaHoradecreacion = resultado.FechaHoradecreacion,
                    Id = resultado.Id
                };
            }
            else
                throw new Exception("El identificador de la tarea enviado no existe");
        }
    }
}
