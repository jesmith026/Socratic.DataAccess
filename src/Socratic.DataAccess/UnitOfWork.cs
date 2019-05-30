using System;
using System.Threading.Tasks;
using Socratic.DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;
using Socratic.DataAccess.Internal;
using System.Data;
using System.Data.Common;
using Dapper;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Socratic.DataAccess
{
    /// <summary>
    /// A wrapper for multiple repository types implementing a common unit of work transaction
    /// </summary>
    /// <typeparam cref="Microsoft.EntityFrameworkCore.DbContext" name="TContext"></typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable
        where TContext : DbContext
    {
        private TContext efContext;
        private readonly IRepositoryFactory<TContext> repoFactory;           

        public UnitOfWork(TContext dbContext, IRepositoryFactory<TContext> repoFactory)
        {
            this.repoFactory = repoFactory;
            this.efContext = dbContext;
        }        

        /// <summary>
        /// Instance of the IDirectRepository interface
        /// </summary>
        /// <returns></returns>
        public IDirectRepository Database => repoFactory.Get();

        /// <summary>
        /// Instance of the IRepository interface for the given context and entity
        /// </summary>
        /// <typeparam cref="Microsoft.EntityFrameworkCore.DbContext" name="TContext"></typeparam>
        /// <typeparam name="T"></typeparam>
        public IContextualRepository<TContext, T> Context<T>() where T : class, IDbEntity => repoFactory.Get<T>();

        /// <summary>
        /// Commit the pending transaction within DbContext and save changes
        /// </summary>
        /// <returns></returns>                
        public async Task CommitAsync()
        {
            if (efContext.Database.CurrentTransaction != null)
                efContext.Database.CommitTransaction();

            await efContext.SaveChangesAsync();
        }

        /// <summary>
        /// Roll back the pending transaction within the DbContext and reset changes
        /// </summary>
        public void Rollback()
        {
            if (efContext.Database.CurrentTransaction != null)
                efContext.Database.RollbackTransaction();
            
            efContext.Reset();            
        }
        
        #region IDisposable Implementation
        private bool disposedValue = false;        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (efContext != null)
                    {
                        efContext.Dispose();
                        efContext = null;
                    }
                }                

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);         
        }        
        #endregion
    }
}