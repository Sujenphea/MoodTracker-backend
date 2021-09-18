using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Azure.Cosmos;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.Extensions;
using MoodTrackerBackendCosmos.Models;
using User = MoodTrackerBackendCosmos.Models.User;
using System.Security.Claims;
using System.Threading;
using HotChocolate.AspNetCore;

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
                Description = input.description,
                UserId = input.userId,
                DateCreated = DateTime.Now.ToString("dd.MM.yyyy")
            };

            //await context.AddAsync(u);
            await context.Dailies.AddAsync(daily);
            await context.SaveChangesAsync();

            return daily;
        }

        [UseAppDbContext]
        //[Authorize]
        public async Task<Daily> EditDailyAsync(EditDailyInput input, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var daily = await context.Dailies.FindAsync(input.id);

            daily.Description = input.description ?? daily.Description;

            //await context.AddAsync(u);
            //await context.Dailies.Save;
            await context.SaveChangesAsync();

            return daily;
        }
    }

    public record AddDailyInput(string description, string userId);
    public record EditDailyInput(string id, string? description, string? userId);
}
