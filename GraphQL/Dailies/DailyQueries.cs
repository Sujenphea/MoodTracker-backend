using System.Linq;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.Extensions;
using MoodTrackerBackendCosmos.Models;
using HotChocolate;
using HotChocolate.Types;

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
        public IQueryable<Daily> GetDailiesByUserId(string id, [ScopedService] AppDbContext context)
        {
            return context.Dailies.Where(s => s.UserId == id);
        }
    }
}
