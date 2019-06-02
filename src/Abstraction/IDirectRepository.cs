using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socratic.DataAccess.Abstractions
{
    public interface IDirectRepository
    {
        Task<IEnumerable<T>> QueryListAsync<T>(IStoredProcedure<T> storedProc);
        Task<T> QuerySingleAsync<T>(IStoredProcedure<T> storedProc);
        Task<int> ExecuteAsync(IStoredProcedure<int> storedProc);
    }
}