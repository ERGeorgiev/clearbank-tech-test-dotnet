using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Data;

public class AccountDataStoreFactoryTests
{
    [Theory]
    [InlineData(DataStoreType.Default)]
    [InlineData(DataStoreType.Backup)]
    public void Create_WhenConfigIsValid_ReturnsCorrectDataStoreForConfigValue(DataStoreType dataStoreType)
    {
        var factory = new AccountDataStoreFactory(ServiceProvider, BuildConfig(dataStoreType.ToString()));

        var result = factory.Create();

        Assert.IsAssignableFrom<IAccountDataStore>(result);
    }

    [Fact]
    public void Create_WhenConfigIsMissing_DefaultsToAccountDataStore()
    {
        var factory = new AccountDataStoreFactory(ServiceProvider, BuildConfig());

        var result = factory.Create();

        Assert.IsType<AccountDataStore>(result);
    }

    [Fact]
    public void Create_WhenConfigIsInvalid_ThrowsArgumentException()
    {
        var factory = new AccountDataStoreFactory(ServiceProvider, BuildConfig("Bckup"));

        Assert.Throws<ArgumentException>(factory.Create);
    }

    private static IConfiguration BuildConfig(string? dataStoreType = null)
    {
        var config = new ConfigurationBuilder();
        if (dataStoreType != null)
            config.AddInMemoryCollection([KeyValuePair.Create<string, string?>("DataStoreType", dataStoreType)]);
        return config.Build();
    }

    private static IServiceProvider ServiceProvider
    {
        get
        {
            var services = new ServiceCollection();
            services.AddKeyedScoped<IAccountDataStore, AccountDataStore>(DataStoreType.Default);
            services.AddKeyedScoped<IAccountDataStore, BackupAccountDataStore>(DataStoreType.Backup);
            return services.BuildServiceProvider();
        }
    }
}
