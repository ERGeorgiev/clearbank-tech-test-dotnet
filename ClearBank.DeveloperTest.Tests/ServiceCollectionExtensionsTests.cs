using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ClearBank.DeveloperTest.Tests;

public class ServiceCollectionExtensionsTests
{
    private readonly ServiceProvider _provider;

    public ServiceCollectionExtensionsTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        services.AddPaymentServices();
        _provider = services.BuildServiceProvider();
    }

    [Fact]
    public void AddPaymentServices_ResolvesPaymentService()
    {
        var service = _provider.GetService<IPaymentService>();

        Assert.IsType<PaymentService>(service);
    }

    [Fact]
    public void AddPaymentServices_ResolvesAccountDataStoreFactory()
    {
        var factory = _provider.GetService<IAccountDataStoreFactory>();

        Assert.IsType<AccountDataStoreFactory>(factory);
    }

    [Fact]
    public void AddPaymentServices_ResolvesPaymentValidatorFactory()
    {
        var factory = _provider.GetService<IPaymentValidatorFactory>();

        Assert.IsType<PaymentValidatorFactory>(factory);
    }

    [Fact]
    public void AddPaymentServices_ResolvesKeyedAccountDataStoreForAllDataStoreTypes()
    {
        Assert.All(Enum.GetValues<DataStoreType>(), key =>
            Assert.IsAssignableFrom<IAccountDataStore>(_provider.GetRequiredKeyedService<IAccountDataStore>(key)));
    }

    [Fact]
    public void AddPaymentServices_ResolvesKeyedPaymentValidatorForAllPaymentSchemes()
    {
        Assert.All(Enum.GetValues<PaymentScheme>(), scheme =>
            Assert.IsAssignableFrom<IPaymentValidator>(_provider.GetRequiredKeyedService<IPaymentValidator>(scheme)));
    }
}
