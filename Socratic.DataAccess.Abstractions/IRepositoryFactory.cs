using Microsoft.EntityFrameworkCore;

namespace Socratic.DataAccess.Abstractions
{
    public interface IRepositoryFactory<TContext> where TContext : DbContext
    {
         IRepository<TContext, TEntity> Get<TEntity>() where TEntity : class, IDbEntity;
         IDirectRepository Get();
    }
}