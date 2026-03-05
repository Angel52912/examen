using Microsoft.Extensions.DependencyInjection;
using UtmMarket.Infrastructure.Data;
using UtmMarket.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using UtmMarket.Core.Repositories;
using UtmMarket.Infrastructure.Repositories;

namespace UtmMarket.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(options =>
            configuration.GetSection(DatabaseOptions.SectionName).Bind(options));

        services.AddOptions<DatabaseOptions>()
                .Bind(configuration.GetSection(DatabaseOptions.SectionName))
                .ValidateOnStart();

        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

        // Register Product Repository
        services.AddScoped<IProductRepository, ProductRepositoryImpl>();

        // Register Sale Repository
        services.AddScoped<ISaleRepository, SaleRepositoryImpl>();

        // Register Customer Repository (Ejercicio 1)
        services.AddScoped<ICustomerRepository, CustomerRepositoryImpl>();

        return services;
    }
}