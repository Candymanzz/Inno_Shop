using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UserService.Domain.Interfaces;

namespace UserService.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => Guid.Parse(httpContextAccessor
            .HttpContext!
            .User.FindFirst("userId")!
            .Value ?? Guid.Empty.ToString());

        public string Role => httpContextAccessor
            .HttpContext!
            .User.FindFirst(ClaimTypes.Role)!
            .Value ?? string.Empty;
    }
}
