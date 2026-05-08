using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class ChapsPaymentValidatorTests
{
    private readonly ChapsPaymentValidator _sut = new();

    [Fact]
    public void Scheme_ReturnsChaps()
    {
        Assert.Equal(PaymentScheme.Chaps, _sut.Scheme);
    }

    [Fact]
    public void IsValid_StatusIsDisabled_ReturnsFalse()
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
    public void IsValid_StatusIsInboundPaymentsOnly_ReturnsFalse()
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
    public void IsValid_StatusIsLive_ReturnsTrue()
    {
        var account = new Account
        {
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Chaps },
            Status = AccountStatus.Live
        };

        var result = _sut.IsValid(account, CreateRequest());

        Assert.True(result);
    }

    private static MakePaymentRequest CreateRequest() => new()
    {
        PaymentScheme = PaymentScheme.Chaps
    };
}
