using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class PaymentServiceTests
{
    private const string _accNoA = "1";
    private const string _accNoB = "2";
    private readonly Mock<IAccountDataStore> _dataStoreMock = new();
    private readonly Mock<IAccountDataStoreFactory> _factoryMock = new();
    private readonly Mock<IPaymentValidatorFactory> _validatorFactoryMock = new();
    private readonly Mock<IPaymentValidator> _validatorMock = new();
    private readonly PaymentService _sut;
    private readonly Account _account = new() { 
        AccountNumber = _accNoA, 
        AllowedPaymentSchemes = new HashSet<PaymentScheme>(), 
        Balance = 500m 
    };
    private readonly MakePaymentRequest _request = new()
    {
        CreditorAccountNumber = _accNoA,
        DebtorAccountNumber = _accNoB,
        Amount = 100m,
        PaymentScheme = PaymentScheme.Bacs
    };

    public PaymentServiceTests()
    {
        _factoryMock.Setup(f => f.Create()).Returns(_dataStoreMock.Object);
        _dataStoreMock.Setup(ds => ds.GetAccount(_request.DebtorAccountNumber)).Returns(_account);
        _validatorFactoryMock.Setup(f => f.GetValidator(It.IsAny<PaymentScheme>())).Returns(_validatorMock.Object);
        _sut = new PaymentService(_factoryMock.Object, _validatorFactoryMock.Object);
    }

    [Fact]
    public void MakePayment_WhenValidationPasses_ReturnsSuccess()
    {
        _validatorMock.Setup(v => v.IsValid(_account, _request)).Returns(true);

        var result = _sut.MakePayment(_request);

        Assert.True(result.Success);
    }

    [Fact]
    public void MakePayment_WhenValidationFails_ReturnsFailure()
    {
        _validatorMock.Setup(v => v.IsValid(_account, _request)).Returns(false);

        var result = _sut.MakePayment(_request);

        Assert.False(result.Success);
    }

    [Fact]
    public void MakePayment_WhenValidationPasses_DeductsAmountFromBalance()
    {
        _request.Amount = 200m;
        _validatorMock.Setup(v => v.IsValid(_account, _request)).Returns(true);

        _sut.MakePayment(_request);

        Assert.Equal(300m, _account.Balance);
    }

    [Fact]
    public void MakePayment_WhenValidationPasses_UpdatesAccountInDataStore()
    {
        _validatorMock.Setup(v => v.IsValid(_account, _request)).Returns(true);

        _sut.MakePayment(_request);

        _dataStoreMock.Verify(ds => ds.UpdateAccount(_account), Times.Once);
    }

    [Fact]
    public void MakePayment_WhenValidationFails_DoesNotUpdateAccount()
    {
        _validatorMock.Setup(v => v.IsValid(_account, _request)).Returns(false);

        _sut.MakePayment(_request);

        _dataStoreMock.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
    }

    [Fact]
    public void MakePayment_WhenValidationFails_DoesNotDeductBalance()
    {
        _request.Amount = 200m;
        _validatorMock.Setup(v => v.IsValid(_account, _request)).Returns(false);

        _sut.MakePayment(_request);

        Assert.Equal(500m, _account.Balance);
    }

    [Fact]
    public void MakePayment_LooksUpAccountByDebtorAccountNumber()
    {
        _request.DebtorAccountNumber = _accNoB;
        _dataStoreMock.Setup(ds => ds.GetAccount(_accNoB)).Returns(
            new Account() { AccountNumber = _accNoB, AllowedPaymentSchemes = new HashSet<PaymentScheme>() });
        _validatorMock.Setup(v => v.IsValid(It.IsAny<Account>(), _request)).Returns(false);

        _sut.MakePayment(_request);

        _dataStoreMock.Verify(ds => ds.GetAccount(_accNoB), Times.Once);
    }

    [Fact]
    public void MakePayment_ResolvesValidatorForCorrectScheme()
    {
        _request.PaymentScheme = PaymentScheme.Chaps;
        _validatorMock.Setup(v => v.IsValid(_account, _request)).Returns(false);

        _sut.MakePayment(_request);

        _validatorFactoryMock.Verify(f => f.GetValidator(PaymentScheme.Chaps), Times.Once);
    }
}
