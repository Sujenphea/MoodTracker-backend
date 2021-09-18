using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.Extensions;
using MoodTrackerBackendCosmos.Models;
using System.Threading;
using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using HotChocolate.AspNetCore;

namespace MoodTrackerBackendCosmos.GraphQL.Dailies
{
    // have to be async
    // https://www.thereformedprogrammer.net/an-in-depth-study-of-cosmos-db-and-ef-core-3-0-database-provider/
    [ExtendObjectType(name: "Mutation")]
    public class DailyMutations
    {
        [UseAppDbContext]
        [Authorize]
        public async Task<Daily> AddDailyAsync(AddDailyInput input, ClaimsPrincipal claimsPrincipal, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var userId = claimsPrincipal.Claims.First(c => c.Type == "userId").Value;
            var daily = new Daily
            {
                Id = Guid.NewGuid().ToString(),
                Description = input.Description,
                UserId = userId,
                DateCreated = DateTime.Now.ToString("dd.MM.yyyy")
            };

            await context.Dailies.AddAsync(daily);
            await context.SaveChangesAsync();

            return daily;
        }

        [UseAppDbContext]
        [Authorize]
        public async Task<Daily> EditDailyAsync(EditDailyInput input, ClaimsPrincipal claimsPrincipal, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var userId = claimsPrincipal.Claims.First(c => c.Type == "userId").Value;
            var daily = await context.Dailies.FindAsync(input.Id);

            if (daily.UserId != userId)
            {
                throw new GraphQLRequestException(ErrorBuilder.New()
                    .SetMessage("Not owned by user")
                    .SetCode("AUTH_NOT_AUTHORIZED")
                    .Build());
            }

            daily.Description = input.Description ?? daily.Description;

            await context.SaveChangesAsync();

            return daily;
        }
    }

    public record AddDailyInput(string Description);
    #nullable enable
    public record EditDailyInput(string Id, string? Description, string? UserId);
}
