using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest.Validators;

public class PaymentValidatorFactory(IServiceProvider serviceProvider)
    : IPaymentValidatorFactory
{
    public IPaymentValidator GetValidator(PaymentScheme scheme)
    {
        return serviceProvider.GetRequiredKeyedService<IPaymentValidator>(scheme);
    }
}
