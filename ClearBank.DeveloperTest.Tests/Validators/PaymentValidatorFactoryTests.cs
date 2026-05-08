using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class PaymentValidatorFactoryTests
{
    private readonly PaymentValidatorFactory _sut;

    public PaymentValidatorFactoryTests()
    {
        var services = new ServiceCollection();
        services.AddKeyedScoped<IPaymentValidator, BacsPaymentValidator>(PaymentScheme.Bacs);
        services.AddKeyedScoped<IPaymentValidator, FasterPaymentsPaymentValidator>(PaymentScheme.FasterPayments);
        _sut = new PaymentValidatorFactory(services.BuildServiceProvider());
    }

    [Fact]
    public void GetValidator_RegisteredScheme_ReturnsMatchingValidator()
    {
        var validator = Assert.IsAssignableFrom<PaymentValidatorBase>(_sut.GetValidator(PaymentScheme.Bacs));
        Assert.Equal(PaymentScheme.Bacs, validator.Scheme);
    }

    [Fact]
    public void GetValidator_UnregisteredScheme_ThrowsInvalidOperationException()
    {
        var factory = new PaymentValidatorFactory(new ServiceCollection().BuildServiceProvider());

        Assert.Throws<InvalidOperationException>(() => factory.GetValidator(PaymentScheme.Bacs));
    }
}
