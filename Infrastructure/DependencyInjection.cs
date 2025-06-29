using Application.Interfaces;
using Application.Services;
using Infrastructure.Services;
using Infrastructure.Persistence;
using Domain.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ApiWebPulso.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOptions>(config.GetSection("Jwt"));

            #region Services
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuditService, AuditService>();
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPunchService, PunchService>();
            services.AddScoped<IReportService, ReportService>();
            #endregion

            #region Repositories
            services.AddScoped<IAuthRepository, AuthRepository>();
            #endregion

            return services;
        }
    }
}
