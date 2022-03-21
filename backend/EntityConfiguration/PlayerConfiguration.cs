using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.EntityConfiguration
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(p => p.Password).IsRequired().HasMaxLength(128);
            builder.Property(p => p.Username).IsRequired().HasMaxLength(128);
            builder.Property(p => p.ArmyCode).HasMaxLength(128);
            builder.Property(p => p.Displayname).HasMaxLength(128);
            builder.Property(p => p.PlayerCode).HasMaxLength(128);
            builder.Property(p => p.Cookie).HasMaxLength(1024);
        }
    }
}