using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class BacsPaymentValidatorTests
{
    private readonly BacsPaymentValidator _sut = new();

    [Fact]
    public void IsValid_AccountIsNull_ReturnsFalse()
    {
        var result = _sut.IsValid(null!, CreateRequest());

        Assert.False(result);
    }

    [Fact]
    public void IsValid_AccountDoesNotAllowBacs_ReturnsFalse()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments }
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.False(result);
    }

    [Fact]
    public void IsValid_AccountAllowsBacs_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Bacs }
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.True(result);
    }

    [Fact]
    public void IsValid_AccountAllowsMultipleSchemesIncludingBacs_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Bacs, PaymentScheme.FasterPayments }
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.True(result);
    }

    private static MakePaymentRequest CreateRequest(decimal amount = 100m) => new()
    {
        Amount = amount,
        PaymentScheme = PaymentScheme.Bacs
    };
}
