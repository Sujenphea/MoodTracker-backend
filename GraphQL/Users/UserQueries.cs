using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.Extensions;
using User = MoodTrackerBackendCosmos.Models.User;
using System.Security.Claims;
using HotChocolate.AspNetCore.Authorization;

namespace MoodTrackerBackendCosmos.GraphQL.Users
{
    [ExtendObjectType(name: "Query")]
    public class UserQueries
    {
        [UseAppDbContext]
        public DbSet<User> GetUsers([ScopedService] AppDbContext context)
        {
            return context.Users;
        }

        [UseAppDbContext]
        public User GetUser(int id, [ScopedService] AppDbContext context)
        {
            return context.Users.Find(id);
        }

        [UseAppDbContext]
        [Authorize]
        public User GetSelf(ClaimsPrincipal claimsPrincipal, [ScopedService] AppDbContext context)
        {
            var userIdStr = claimsPrincipal.Claims.First(c => c.Type == "userId").Value;

            return context.Users.Find(userIdStr);
        }
    }
}



//var sqlQueryText = "SELECT * FROM c";
//QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
//FeedIterator<User> queryResultSetIterator = context.container.GetItemQueryIterator<User>(queryDefinition);

//List<User> families = new List<User>();

//while (queryResultSetIterator.HasMoreResults)
//{
//    FeedResponse<User> currentResultSet = await queryResultSetIterator.ReadNextAsync();
//    foreach (User family in currentResultSet)
//    {
//        families.Add(family);
//    }
//}

//return families;