using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Date.Configuration
{
    public class PasswordReserTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.ExpiresAt)
                .IsRequired();

            builder.Property(t => t.Used)
                .HasDefaultValue(false);

            builder.HasOne(t => t.User)
                .WithMany(u => u.PasswordResetTokens)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
