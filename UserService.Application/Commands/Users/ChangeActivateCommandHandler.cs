using MediatR;
using System.Net.Http;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Commands.Users
{
    public class ChangeActivateCommandHandler : IRequestHandler<ChangeActivateCommand, Unit>
    {
        private readonly IUserRepository userRepository;
        private readonly IHttpClientFactory httpClientFactory;

        public ChangeActivateCommandHandler(IUserRepository userRepository, IHttpClientFactory httpClientFactory)
        {
            this.userRepository = userRepository;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Unit> Handle(ChangeActivateCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsActive = request.Status;
            await userRepository.ChangeActivateAsync(user);

            HttpClient httpClient = httpClientFactory.CreateClient("ProductService");

            await httpClient.PatchAsync(
                $"api/products/{request.UserId}/visibility?active={request.Status}",
                null,
                cancellationToken
            );

            return Unit.Value;
        }
    }
}
