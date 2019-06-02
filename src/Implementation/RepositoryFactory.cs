using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Socratic.DataAccess.Abstractions;

namespace Socratic.DataAccess
{
    //TODO: using IServiceProvider rather than newing up objects here so that it's more extensible.

    /// <summary>
    /// Dynamically create the requested repositories
    /// </summary>
    /// <typeparam cref="Microsoft.EntityFrameworkCore.DbContext" name="TContext"></typeparam>
    public class RepositoryFactory<TContext> : IRepositoryFactory<TContext>
        where TContext : DbContext
    {
        private readonly TContext context;
        private readonly Dictionary<Type, object> repoDictionary = new Dictionary<Type, object>();
        
        public RepositoryFactory(TContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Provide an instance of IRepository for a given context and entity
        /// </summary>
        /// <typeparam cref="Microsoft.EntityFrameworkCore.DbContext" name="TContext"></typeparam>
        /// <typeparam cref="Socratic.DataAccess.Abstractions.IDbEntity" name="TEntity"></typeparam>
        public IContextualRepository<TContext, TEntity> Get<TEntity>() where TEntity : class, IDbEntity 
        {            
            if (repoDictionary.TryGetValue(typeof(TEntity), out var repo))
                return (IContextualRepository<TContext, TEntity>)repo;
            
            var newRepo = new EfRepository<TContext, TEntity>(context);

            repoDictionary.Add(typeof(TEntity), newRepo);

            return newRepo;
        }

        /// <summary>
        /// Provide an instance of IDirectRepository
        /// </summary>
        /// <returns cref="Socratic.DataAccess.Abstractions.IDirectRepository"></returns>
        public IDirectRepository Get()
        {
            if (repoDictionary.TryGetValue(typeof(IDirectRepository), out var repo))
                return (IDirectRepository)repo;
            
            var newRepo = new DirectDapperRepository(context);

            repoDictionary.Add(typeof(IDirectRepository), newRepo);

            return newRepo;
        }
    }
}