using Microsoft.EntityFrameworkCore;

namespace Socratic.DataAccess.Abstractions
{
    public interface IRepositoryFactory<TContext> where TContext : DbContext
    {
         IContextualRepository<TContext, TEntity> Get<TEntity>() where TEntity : class, IDbEntity;
         IDirectRepository Get();
    }
}