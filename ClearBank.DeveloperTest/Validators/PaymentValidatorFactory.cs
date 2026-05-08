using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ClearBank.DeveloperTest.Validators;

public class PaymentValidatorFactory(IServiceProvider serviceProvider)
    : IPaymentValidatorFactory
{
    public IPaymentValidator GetValidator(PaymentScheme scheme)
    {
    }
}
