using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Socratic.DataAccess.Abstractions
{
    public interface IContextualRepository<TContext, TEntity> 
        where TContext : DbContext
        where TEntity : class, IDbEntity
    {
        IQueryable<TEntity> GetAll();
        
        Task<TEntity> GetByIdAsync(int id);
        TEntity GetById(int id) => GetByIdAsync(id).Result;

        Task AddAsync(TEntity entity);
        void Add(TEntity entity) => AddAsync(entity).Wait();

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteAsync(int id);
    }
}