using Core.Model.Dto;
using Core.Model.Entity;

namespace Core.DAL.Tareas
{
    public interface ITareaRepository : IQueryable<Tarea>
    {
        Task AddAsync(Tarea tarea);

        Task UpdateAsync(Tarea tarea);

        Task DeleteAsync(int id);

        Task<int> AddRangeAsync(IEnumerable<Tarea> tareas);

        Task<Tarea> GetAsync(string accion);

        Task<Tarea> GetAsync(int id);
    }
}