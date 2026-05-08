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
        services.AddKeyedScoped<IPaymentValidator, ChapsPaymentValidator>(PaymentScheme.Chaps);

        var serviceProvider = services.BuildServiceProvider();
        _sut = new PaymentValidatorFactory(serviceProvider);
    }

    [Theory]
    [InlineData(PaymentScheme.Bacs, typeof(BacsPaymentValidator))]
    [InlineData(PaymentScheme.FasterPayments, typeof(FasterPaymentsPaymentValidator))]
    [InlineData(PaymentScheme.Chaps, typeof(ChapsPaymentValidator))]
    public void GetValidator_ReturnsCorrectValidatorForScheme(PaymentScheme scheme, Type expectedType)
    {
        var validator = _sut.GetValidator(scheme);

        Assert.IsType(expectedType, validator);
    }

    [Fact]
    public void GetValidator_UnregisteredScheme_ThrowsInvalidOperationException()
    {
        var factory = new PaymentValidatorFactory(new ServiceCollection().BuildServiceProvider());

        Assert.Throws<InvalidOperationException>(() => factory.GetValidator(PaymentScheme.Bacs));
    }
}
