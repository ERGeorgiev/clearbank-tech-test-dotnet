using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class ChapsPaymentValidatorTests
{
    private readonly string _accNoA = "1";
    private readonly string _accNoB = "2";
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
            AccountNumber = _accNoA,
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Chaps },
            Status = AccountStatus.Disabled
        };

        var result = _sut.IsValid(account, CreateRequest(_accNoA, _accNoB));

        Assert.False(result);
    }

    [Fact]
    public void IsValid_StatusIsInboundPaymentsOnly_ReturnsFalse()
    {
        var account = new Account
        {
            AccountNumber = _accNoA,
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Chaps },
            Status = AccountStatus.InboundPaymentsOnly
        };

        var result = _sut.IsValid(account, CreateRequest(_accNoA, _accNoB));

        Assert.False(result);
    }

    [Fact]
    public void IsValid_StatusIsLive_ReturnsTrue()
    {
        var account = new Account
        {
            AccountNumber = _accNoA,
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Chaps },
            Status = AccountStatus.Live
        };

        var result = _sut.IsValid(account, CreateRequest(_accNoA, _accNoB));

        Assert.True(result);
    }

    private static MakePaymentRequest CreateRequest(string creditorNo, string deptorNo) => new()
    {
        CreditorAccountNumber = creditorNo,
        DebtorAccountNumber = deptorNo,
        PaymentScheme = PaymentScheme.Chaps
    };
}
