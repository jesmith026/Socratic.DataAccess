using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Socratic.DataAccess.Abstractions
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {         
        IDirectRepository Database { get; }
        
        IContextualRepository<TContext, TEntity> Context<TEntity>() where TEntity : class, IDbEntity;        

        Task CommitAsync();

        void Rollback();
    }
}