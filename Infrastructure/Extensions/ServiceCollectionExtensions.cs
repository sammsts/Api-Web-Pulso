using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbProvider = configuration["DatabaseProvider"] ?? "SqlServer";
        var connStr = configuration.GetConnectionString("DefaultConnection")!;

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            switch (dbProvider)
            {
                case "PostgreSQL":
                    options.UseNpgsql(connStr);
                    break;
                case "Sqlite":
                    options.UseSqlite(connStr);
                    break;
                default:
                    options.UseSqlServer(connStr);
                    break;
            }
        });

        return services;
    }
}
