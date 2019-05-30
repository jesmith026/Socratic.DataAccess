using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Socratic.DataAccess.Abstractions;

namespace Socratic.DataAccess
{
    /// <summary>
    /// Implementation of IRepository via Entity Framework
    /// </summary>
    /// <typeparam cref="Microsoft.EntityFrameworkCore.DbContext" name="TContext">
    ///     An extension of Entity Framework's DbContext class
    /// </typeparam>
    /// <typeparam cref="Socratic.DataAccess.Abstractions.IDbEntity" name="TEntity">
    ///     An implementation of the IDbEntity interface
    /// </typeparam>
    public class EfRepository<TContext, TEntity> : IContextualRepository<TContext, TEntity> 
        where TEntity : class, IDbEntity 
        where TContext : DbContext
    {
        public EfRepository(TContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            
            DbSet = DbContext.Set<TEntity>();
        }

        protected TContext DbContext { get; set; }
        protected DbSet<TEntity> DbSet { get; set; }

        /// <summary>
        /// Get the DbSet as an IQueryable
        /// </summary>
        /// <returns cref="System.Linq.IQueryable{TEntity}"></returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        /// <summary>
        /// Get the entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
           return await DbSet.FindAsync(id);
        }

        /// <summary>
        /// Add the entity to the database context
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(TEntity entity)
        {
            EntityEntry<TEntity> dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Detached)
                dbEntityEntry.State = EntityState.Added;
            else
                await DbSet.AddAsync(entity);
        }

        /// <summary>
        /// Update the entity in the database context
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            EntityEntry<TEntity> dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Detached)
                DbSet.Attach(entity);

            dbEntityEntry.State = EntityState.Modified;
        }        

        /// <summary>
        /// Delete the entity in the database context
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            EntityEntry<TEntity> dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
                dbEntityEntry.State = EntityState.Deleted;
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        /// <summary>
        /// Delete the entity in the database context by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async void DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            Delete(entity);
        }
    }    
}