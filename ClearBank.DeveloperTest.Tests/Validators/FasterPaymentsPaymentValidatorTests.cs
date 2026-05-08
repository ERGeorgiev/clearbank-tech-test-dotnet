using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class FasterPaymentsPaymentValidatorTests
{
    private readonly FasterPaymentsPaymentValidator _sut = new();

    [Fact]
    public void IsValid_AccountIsNull_ReturnsFalse()
    {
        var result = _sut.IsValid(null!, CreateRequest());

        Assert.False(result);
    }

    [Fact]
    public void IsValid_AccountDoesNotAllowFasterPayments_ReturnsFalse()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Bacs },
            Balance = 500m
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.False(result);
    }

    [Fact]
    public void IsValid_BalanceLessThanAmount_ReturnsFalse()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments },
            Balance = 50m
        };

        var result = _sut.IsValid(account, CreateRequest(amount: 100m));

        Assert.False(result);
    }

    [Fact]
    public void IsValid_AllConditionsMet_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments },
            Balance = 500m
        };

        var result = _sut.IsValid(account, CreateRequest(amount: 100m));

        Assert.True(result);
    }

    [Fact]
    public void IsValid_AccountAllowsMultipleSchemesIncludingChaps_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments, PaymentScheme.Chaps }
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.True(result);
    }

    [Fact]
    public void IsValid_BalanceEqualsAmount_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments },
            Balance = 100m
        };

        var result = _sut.IsValid(account, CreateRequest(amount: 100m));

        Assert.True(result);
    }

    [Fact]
    public void IsValid_BalanceAndAmountAreZero_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments },
            Balance = 0
        };

        var result = _sut.IsValid(account, CreateRequest(amount: 0));

        Assert.True(result);
    }

    private static MakePaymentRequest CreateRequest(decimal amount = 100m) => new()
    {
        Amount = amount,
        PaymentScheme = PaymentScheme.FasterPayments
    };
}
