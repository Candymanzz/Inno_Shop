using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Date.Configuration
{
    public class EmailConfirmationConfiguration : IEntityTypeConfiguration<EmailConfirmation>
    {
        public void Configure(EntityTypeBuilder<EmailConfirmation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                   .WithOne(u => u.EmailConfirmation)
                   .HasForeignKey<EmailConfirmation>(x => x.UserId);
        }
    }
}
