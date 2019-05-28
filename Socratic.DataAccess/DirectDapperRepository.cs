using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Socratic.DataAccess.Abstractions;
using Socratic.DataAccess.Internal;

namespace Socratic.DataAccess
{
    /// <summary>
    /// Implementation of IDirectRepository using Dapper with connection/transaction management via
    /// Entity Framework's DbContext
    /// </summary>
    public class DirectDapperRepository : IDirectRepository, IDisposable
    {
        private readonly DbContext context;

        public DirectDapperRepository(DbContext context)
        {
            this.context = context;
        }
        
        /// <summary>
        /// Execute the stored procedure represented by the provided parameter and return an IEnumerable of the expected type
        /// </summary>
        /// <param cref="Socratic.DataAccess.Abstractions.IStoredProcedure{T}" name="storedProc"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>A sequence of data of T</returns>
        public async Task<IEnumerable<T>> QueryListAsync<T>(IStoredProcedure<T> storedProc)
        {
            var procInfo = StoredProcedureConfig.Build(storedProc);
            
            await BuildTransactionalConnectionAsync();
            return await context.Database.GetDbConnection().QueryAsync<T>(procInfo.Name, procInfo.Parameters, context.Database.CurrentTransaction.GetDbTransaction(), commandType: CommandType.StoredProcedure);            
        }

        /// <summary>
        /// Execute the stored procedure represented by the provided parameter and return an instance of the expected type
        /// </summary>
        /// <param cref="Socratic.DataAccess.Abstractions.IStoredProcedure{T}" name="storedProc"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>T</returns>
        public async Task<T> QuerySingleAsync<T>(IStoredProcedure<T> storedProc)
        {
            var results = await QueryListAsync(storedProc);
            return results.SingleOrDefault();
        }

        /// <summary>
        /// Execute the stored procedure represented by the provided parameter and return the count of records affected
        /// </summary>
        /// <param cref="Socratic.DataAccess.Abstractions.IStoredProcedure{T}" name="storedProc"></param>
        /// <returns>Count of affected records</returns>
        public async Task<int> ExecuteAsync(IStoredProcedure<int> storedProc) // ToDo: common response object
        {
            var procInfo = StoredProcedureConfig.Build(storedProc);

            await BuildTransactionalConnectionAsync();

            return await context.Database.GetDbConnection().ExecuteAsync(procInfo.Name, procInfo.Parameters, context.Database.CurrentTransaction.GetDbTransaction(), commandType: CommandType.StoredProcedure);            
        }
        
        private async Task BuildTransactionalConnectionAsync()
        {            
            if (context.Database.GetDbConnection().State == ConnectionState.Broken)
            {
                ClearTransaction();
                context.Database.CloseConnection();                
            }
            if (context.Database.GetDbConnection().State == ConnectionState.Closed)
            {
                ClearTransaction();
                await context.Database.OpenConnectionAsync();
            }
            
            if (context.Database.CurrentTransaction == null)
                await context.Database.BeginTransactionAsync();
        }

        private void ClearTransaction()
        {
            var transaction = context.Database.CurrentTransaction;
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (context != null)
                    {
                        context.Dispose();
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