using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using MoodTrackerBackendCosmos.Models;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.Extensions;

namespace MoodTrackerBackendCosmos.GraphQL.UserGraph
{
    [ExtendObjectType(name: "Mutation")]
    public class UserMutations
    {
        [UseAppDbContext]
        public async Task<User> AddUserAsync(UserInput input, [ScopedService] AppDbContext context, CancellationToken cancellationToken)
        {
            var user = new User {
                Id = Guid.NewGuid().ToString(),
                Name = input.Name,
                GitHub = input.GitHub,
                ImageURI = input.ImageURI
            };

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }
    }

    public record UserInput(string Name, string GitHub, string ImageURI);
    public record LoginInput(string Code);
    public record LoginPayload(User User, string Jwt);
}
