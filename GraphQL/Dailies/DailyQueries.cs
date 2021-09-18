using System.Linq;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.Extensions;
using MoodTrackerBackendCosmos.Models;
using HotChocolate;
using HotChocolate.Types;
using MoodTrackerBackendCosmos.GraphQL.DataLoader;
using System.Threading;
using System.Threading.Tasks;

namespace MoodTrackerBackendCosmos.GraphQL.Dailies
{
    [ExtendObjectType(name: "Query")]
    public class DailyQueries
    {
        [UseAppDbContext]
        public IQueryable<Daily> GetDailies([ScopedService] AppDbContext context)
        {
            return context.Dailies;
        }

        [UseAppDbContext]
        public Task<Daily[]> GetDailiesByUserId(string id, DailiesByUserIdDataLoader dataLoader, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            return dataLoader.LoadAsync(id, cancellationToken);
        }
    }
}
