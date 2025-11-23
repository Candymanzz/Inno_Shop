using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Date;

namespace UserService.Infrastructure.Repositories
{
    internal class EmailConfirmationRepository : IEmailConfirmationRepository
    {
        private readonly AppDbContext context;

        public EmailConfirmationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(EmailConfirmation confirmation)
        {
            await context.EmailConfirmations.AddAsync(confirmation);
            await SaveChangesAsync();
        }

        public async Task<EmailConfirmation?> GetByTokenAsync(string token)
        {
            return await context.EmailConfirmations.FirstOrDefaultAsync(e => e.Token == token);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
