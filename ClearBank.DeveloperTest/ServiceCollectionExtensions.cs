using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services)
    {
        return services;
    }
}
