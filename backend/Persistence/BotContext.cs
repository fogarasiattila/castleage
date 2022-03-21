using backend.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using webbot.Persistence;

namespace backend.Persistence
{
    public class BotContext : DbContext
    {
        public BotContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            SeedGroups(modelBuilder);
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Settings> Settings { get; set; }

        private void SeedGroups(ModelBuilder builder)
        {
            builder.Entity<Group>().HasData(
                new Group { Name = "Mindenki", Id = 1 }
            );
        }
    }
}