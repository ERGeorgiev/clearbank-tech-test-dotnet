using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class PaymentValidatorBaseTests
{
    private const string _accNoA = "1";
    private const string _accNoB = "2";
    private readonly BacsPaymentValidator _sut = new();
    private readonly MakePaymentRequest _requestBacs = new()
    {
        CreditorAccountNumber = _accNoA,
        DebtorAccountNumber = _accNoB,
        PaymentScheme = PaymentScheme.Bacs 
    };

    [Fact]
    public void IsValid_AccountIsNull_ReturnsFalse()
    {
        var result = _sut.IsValid(null!, _requestBacs);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_SchemeNotAllowed_ReturnsFalse()
    {
        var account = new Account
        {
            AccountNumber = _accNoA,
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Chaps }
        };

        var result = _sut.IsValid(account, _requestBacs);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_SchemeAllowed_ReturnsTrue()
    {
        var account = new Account
        {
            AccountNumber = _accNoA,
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Bacs }
        };

        var result = _sut.IsValid(account, _requestBacs);

        Assert.True(result);
    }

    [Fact]
    public void IsValid_MultipleSchemes_ReturnsTrue()
    {
        var account = new Account
        {
            AccountNumber = _accNoA,
            AllowedPaymentSchemes = new HashSet<PaymentScheme> { PaymentScheme.Bacs, PaymentScheme.Chaps }
        };

        var result = _sut.IsValid(account, _requestBacs);

        Assert.True(result);
    }
}
