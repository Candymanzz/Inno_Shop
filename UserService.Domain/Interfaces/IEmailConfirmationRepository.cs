using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces
{
    public interface IEmailConfirmationRepository
    {
        Task AddAsync(EmailConfirmation emailConfirmation);
        Task<EmailConfirmation?> GetByTokenAsync(string token);
        Task SaveChangesAsync();
    }
}
