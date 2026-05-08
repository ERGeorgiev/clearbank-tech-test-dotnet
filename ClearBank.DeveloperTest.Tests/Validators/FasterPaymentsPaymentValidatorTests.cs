using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class FasterPaymentsPaymentValidatorTests
{
    private readonly FasterPaymentsPaymentValidator _sut = new();
    private readonly MakePaymentRequest _request = new()
    {
        Amount = 100m,
        PaymentScheme = PaymentScheme.FasterPayments
    };

    [Fact]
    public void Scheme_ReturnsFasterPayments()
    {
        Assert.Equal(PaymentScheme.FasterPayments, _sut.Scheme);
    }

    [Fact]
    public void IsValid_BalanceLessThanAmount_ReturnsFalse()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments },
            Balance = 50m
        };

        var result = _sut.IsValid(account, _request);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_BalanceEqualsAmount_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments },
            Balance = 100m
        };

        var result = _sut.IsValid(account, _request);

        Assert.True(result);
    }

    [Fact]
    public void IsValid_BalanceGreaterThanAmount_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments },
            Balance = 500m
        };

        var result = _sut.IsValid(account, _request);

        Assert.True(result);
    }

    [Fact]
    public void IsValid_BalanceAndAmountAreZero_ReturnsTrue()
    {
        _request.Amount = 0;
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.FasterPayments },
            Balance = 0
        };

        var result = _sut.IsValid(account, _request);

        Assert.True(result);
    }
}
