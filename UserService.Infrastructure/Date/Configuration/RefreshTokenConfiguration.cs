using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Date.Configuration
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Token)
                .IsRequired();

            builder.Property(r => r.ExpiresAt)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(r => r.RevokedAt)
                .IsRequired(false);

            builder.HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
