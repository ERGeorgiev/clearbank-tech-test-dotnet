using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentService, PaymentService>();

        services.AddScoped<IAccountDataStoreFactory, AccountDataStoreFactory>();
        services.AddKeyedScoped<IAccountDataStore, AccountDataStore>(DataStoreType.Default);
        services.AddKeyedScoped<IAccountDataStore, BackupAccountDataStore>(DataStoreType.Backup);

        services.AddScoped<IPaymentValidatorFactory, PaymentValidatorFactory>();
        services.AddKeyedScoped<IPaymentValidator, BacsPaymentValidator>(PaymentScheme.Bacs);
        services.AddKeyedScoped<IPaymentValidator, FasterPaymentsPaymentValidator>(PaymentScheme.FasterPayments);
        services.AddKeyedScoped<IPaymentValidator, ChapsPaymentValidator>(PaymentScheme.Chaps);

        return services;
    }
}
