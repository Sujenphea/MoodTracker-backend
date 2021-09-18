using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using User = MoodTrackerBackendCosmos.Models.User;
using System.Linq;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using MoodTrackerBackendCosmos.Extensions;
using MoodTrackerBackendCosmos.Models;

namespace MoodTrackerBackendCosmos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {
            this.Database.EnsureCreated();
        }
        
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Daily> Dailies { get; set; } = default!;

        ////https://stackoverflow.com/questions/48743165/toarrayasync-throws-the-source-iqueryable-doesnt-implement-iasyncenumerable
        //public IQueryable<User>? Users { get; set; } = new AsyncEnumerableQuery<User>(Enumerable.Empty<User>());

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("users");

            modelBuilder.Entity<User>()
                .ToContainer<User>("users")
                .HasPartitionKey(o => o.Name);

            modelBuilder.Entity<Daily>()
                .ToContainer<Daily>("dailies")
                .HasPartitionKey(o => o.UserId);
        }
    }
}