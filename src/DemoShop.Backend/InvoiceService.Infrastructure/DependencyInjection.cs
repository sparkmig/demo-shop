using InvoiceService.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudflareR2Options>(configuration.GetSection("CloudflareR2"));
        services.AddSingleton<IPdfStorageService, CloudflarePdfStorageService>();
        
        return services;
    }
}
