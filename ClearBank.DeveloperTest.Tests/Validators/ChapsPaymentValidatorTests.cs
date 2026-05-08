using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class ChapsPaymentValidatorTests
{
    private readonly ChapsPaymentValidator _sut = new();

    [Fact]
    public void IsValid_AccountIsNull_ReturnsFalse()
    {
        var result = _sut.IsValid(null!, CreateRequest());

        Assert.False(result);
    }

    [Fact]
    public void IsValid_AccountDoesNotAllowChaps_ReturnsFalse()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Bacs },
            Status = AccountStatus.Live
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.False(result);
    }

    [Fact]
    public void IsValid_AccountStatusIsDisabled_ReturnsFalse()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Chaps },
            Status = AccountStatus.Disabled
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.False(result);
    }

    [Fact]
    public void IsValid_AccountStatusIsInboundPaymentsOnly_ReturnsFalse()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Chaps },
            Status = AccountStatus.InboundPaymentsOnly
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.False(result);
    }

    [Fact]
    public void IsValid_AllConditionsMet_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Chaps },
            Status = AccountStatus.Live
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.True(result);
    }

    [Fact]
    public void IsValid_AccountAllowsMultipleSchemesIncludingChaps_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Bacs, PaymentScheme.Chaps }
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.True(result);
    }

    private static MakePaymentRequest CreateRequest(decimal amount = 100m) => new()
    {
        Amount = amount,
        PaymentScheme = PaymentScheme.Chaps
    };
}
