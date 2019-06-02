using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Socratic.DataAccess.Internal
{
    /// <summary>
    /// Class containing useful extensions for DbContext objects
    /// </summary>
    internal static class DbContextExtensions
    {
        /// <summary>
        /// Clear all changes made to the context
        /// </summary>
        /// <param name="context"></param>
        internal static void Reset(this DbContext context)
        {
            var entries = context.ChangeTracker
                                .Entries()
                                .Where(e => e.State != EntityState.Unchanged)
                                .ToArray();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}