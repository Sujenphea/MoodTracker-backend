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
                Name = input.name,
                GitHub = input.gitHub,
                ImageURI = input.imageURI
            };

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }
    }

    public record UserInput(string name, string gitHub, string imageURI);
    public record LoginInput(string code);
    public record LoginPayload(User user, string jwt);
}
