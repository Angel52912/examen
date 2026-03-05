using Microsoft.Extensions.DependencyInjection;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Product Use Cases
        services.AddScoped<ICreateProductUseCase, CreateProductUseCaseImpl>();
        services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCaseImpl>();
        services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCaseImpl>();
        services.AddScoped<IUpdateProductUseCase, UpdateProductUseCaseImpl>();
        services.AddScoped<IUpdateProductStockUseCase, UpdateProductStockUseCaseImpl>();
        services.AddScoped<IFindProductsUseCase, FindProductsUseCaseImpl>();

        // Sale Use Cases
        services.AddScoped<ICreateSaleUseCase, CreateSaleUseCaseImpl>();
        services.AddScoped<IFetchAllSalesUseCase, FetchAllSalesUseCaseImpl>();
        services.AddScoped<IFetchSaleByIdUseCase, FetchSaleByIdUseCaseImpl>();
        services.AddScoped<IFetchSalesByFilterUseCase, FetchSalesByFilterUseCaseImpl>();
        services.AddScoped<IUpdateSaleStatusUseCase, UpdateSaleStatusUseCaseImpl>();

        // Orchestrators
        services.AddScoped<ProductConsoleOrchestrator>();
        services.AddScoped<SaleConsoleOrchestrator>();
        services.AddScoped<CustomerConsoleOrchestrator>();

        return services;
    }
}
