using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UserService.Application.Services;
using UserService.Domain.Interfaces;

namespace UserService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) //перерегать все ди
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
            services.Configure<FrontendOptions>(configuration.GetSection("Frontend"));
            services.AddScoped<IEmailSender, MailHogEmailSender>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            //services.AddScoped<IEmailSender, DebugEmailSender>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddHttpClient("ProductService", client =>
            {
                client.BaseAddress = new Uri("http://productservice:8082");
            });
            return services;
        }
    }
}
