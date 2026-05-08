using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest.Data;

internal class AccountDataStoreFactory(IServiceProvider serviceProvider, IConfiguration config)
    : IAccountDataStoreFactory
{
    public IAccountDataStore Create()
    {
    }
}
