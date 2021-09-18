using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.Extensions;
using MoodTrackerBackendCosmos.Models;
using System.Threading;

namespace MoodTrackerBackendCosmos.GraphQL.Dailies
{
    // have to be async
    // https://www.thereformedprogrammer.net/an-in-depth-study-of-cosmos-db-and-ef-core-3-0-database-provider/
    [ExtendObjectType(name: "Mutation")]
    public class DailyMutations
    {
        [UseAppDbContext]
        //[Authorize]
        public async Task<Daily> AddDailyAsync(AddDailyInput input, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var daily = new Daily
            {
                Id = Guid.NewGuid().ToString(),
                Description = input.Description,
                UserId = input.UserId,
                DateCreated = DateTime.Now.ToString("dd.MM.yyyy")
            };

            await context.Dailies.AddAsync(daily);
            await context.SaveChangesAsync();

            return daily;
        }

        [UseAppDbContext]
        //[Authorize]
        public async Task<Daily> EditDailyAsync(EditDailyInput input, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var daily = await context.Dailies.FindAsync(input.Id);

            daily.Description = input.Description ?? daily.Description;

            await context.SaveChangesAsync();

            return daily;
        }
    }

    public record AddDailyInput(string Description, string UserId);
    public record EditDailyInput(string Id, string? Description, string? UserId);
}
