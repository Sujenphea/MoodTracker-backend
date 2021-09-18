using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.GraphQL.Users;
using MoodTrackerBackendCosmos.Models;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace MoodTrackerBackendCosmos.GraphQL.Dailies
{
    public class DailyType : ObjectType<Daily>
    {
        protected override void Configure(IObjectTypeDescriptor<Daily> descriptor)
        {
            descriptor.Field(s => s.Id).Type<NonNullType<IdType>>();
            descriptor.Field(s => s.Description).Type<NonNullType<StringType>>();
            descriptor.Field(s => s.DateCreated).Type<NonNullType<StringType>>();

            descriptor
                .Field(s => s.User)
                .ResolveWith<Resolvers>(r => r.GetUser(default!, default!, default))
                .UseDbContext<AppDbContext>()
                .Type<NonNullType<UserType>>();
        }

        private class Resolvers
        {
            public async Task<User> GetUser(Daily daily, [ScopedService] AppDbContext context,
                CancellationToken cancellationToken)
            {
                return await context.Users.FindAsync(new object[] { daily.UserId }, cancellationToken);
            }
        }
    }
}
