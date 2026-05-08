using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class PaymentServiceTests
{
    private readonly Mock<IAccountDataStore> _dataStoreMock = new();
    private readonly Mock<IAccountDataStoreFactory> _factoryMock = new();
    private readonly Mock<IPaymentValidatorFactory> _validatorFactoryMock = new();
    private readonly PaymentService _sut;

    public PaymentServiceTests()
    {
        _factoryMock.Setup(f => f.Create()).Returns(_dataStoreMock.Object);
        _sut = new PaymentService(_factoryMock.Object, _validatorFactoryMock.Object);
    }

    private void SetupAccount(Account account, string accountNumber = "12345678")
    {
        _dataStoreMock
          .Setup(ds => ds.GetAccount(accountNumber))
                    .Returns(account);
    }

    private void SetupValidator(PaymentScheme scheme, bool result)
    {
        var validatorMock = new Mock<IPaymentValidator>();
        validatorMock
            .Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<MakePaymentRequest>()))
            .Returns(result);

        _validatorFactoryMock
            .Setup(f => f.GetValidator(scheme))
            .Returns(validatorMock.Object);
    }

    private static MakePaymentRequest CreateRequest(PaymentScheme scheme = PaymentScheme.Bacs,
        decimal amount = 100m, string debtorAccountNumber = "12345678") => new()
        {
            DebtorAccountNumber = debtorAccountNumber,
            Amount = amount,
            PaymentScheme = scheme
        };

    [Fact]
    public void MakePayment_WhenValidationPasses_ReturnsSuccess()
    {
        var account = new Account { Balance = 500m };
        var request = CreateRequest();
        SetupAccount(account);
        SetupValidator(PaymentScheme.Bacs, result: true);

        var result = _sut.MakePayment(request);

        Assert.True(result.Success);
    }

    [Fact]
    public void MakePayment_WhenValidationFails_ReturnsFailure()
    {
        var account = new Account { Balance = 500m };
        var request = CreateRequest();
        SetupAccount(account);
        SetupValidator(PaymentScheme.Bacs, result: false);

        var result = _sut.MakePayment(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void MakePayment_WhenValidationPasses_DeductsAmountFromBalance()
    {
        var account = new Account { Balance = 500m };
        var request = CreateRequest(amount: 200m);
        SetupAccount(account);
        SetupValidator(PaymentScheme.Bacs, result: true);

        _sut.MakePayment(request);

        Assert.Equal(300m, account.Balance);
    }

    [Fact]
    public void MakePayment_WhenValidationPasses_UpdatesAccountInDataStore()
    {
        var account = new Account { Balance = 500m };
        var request = CreateRequest(amount: 100m);
        SetupAccount(account);
        SetupValidator(PaymentScheme.Bacs, result: true);

        _sut.MakePayment(request);

        _dataStoreMock.Verify(ds => ds.UpdateAccount(account), Times.Once);
    }

    [Fact]
    public void MakePayment_WhenValidationFails_DoesNotUpdateAccount()
    {
        var account = new Account { Balance = 500m };
        var request = CreateRequest();
        SetupAccount(account);
        SetupValidator(PaymentScheme.Bacs, result: false);

        _sut.MakePayment(request);

        _dataStoreMock.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
    }

    [Fact]
    public void MakePayment_WhenValidationFails_DoesNotDeductBalance()
    {
        var account = new Account { Balance = 500m };
        var request = CreateRequest(amount: 200m);
        SetupAccount(account);
        SetupValidator(PaymentScheme.Bacs, result: false);

        _sut.MakePayment(request);

        Assert.Equal(500m, account.Balance);
    }

    [Fact]
    public void MakePayment_LooksUpAccountByDebtorAccountNumber()
    {
        const string accountNumber = "99999999";
        var request = CreateRequest(debtorAccountNumber: accountNumber);
        SetupAccount(new Account(), accountNumber);
        SetupValidator(PaymentScheme.Bacs, result: false);

        _sut.MakePayment(request);

        _dataStoreMock.Verify(ds => ds.GetAccount(accountNumber), Times.Once);
    }

    [Fact]
    public void MakePayment_ResolvesValidatorForCorrectScheme()
    {
        var request = CreateRequest(scheme: PaymentScheme.Chaps);
        SetupAccount(new Account());
        SetupValidator(PaymentScheme.Chaps, result: false);

        _sut.MakePayment(request);

        _validatorFactoryMock.Verify(f => f.GetValidator(PaymentScheme.Chaps), Times.Once);
    }
}
