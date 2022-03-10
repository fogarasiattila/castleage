using backend.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

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
        }

        public DbSet<Player> Players { get; set; }
    }
}