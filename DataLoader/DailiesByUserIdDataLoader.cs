using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using MoodTrackerBackendCosmos.Data;
using MoodTrackerBackendCosmos.Models;

namespace MoodTrackerBackendCosmos.GraphQL.DataLoader
{
    public class DailiesByUserIdDataLoader: GroupedDataLoader<string, Daily>
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public DailiesByUserIdDataLoader(IBatchScheduler batchScheduler, IDbContextFactory<AppDbContext> dbContextFactory)
        : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<ILookup<string, Daily>> LoadGroupedBatchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            await using AppDbContext dbContext = _dbContextFactory.CreateDbContext();

            var x = await dbContext.Dailies.ToListAsync();

            return x.ToLookup(x => x.UserId);
        }
    }
}
