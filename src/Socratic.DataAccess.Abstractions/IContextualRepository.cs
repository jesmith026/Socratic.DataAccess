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

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void DeleteAsync(int id);
    }
}