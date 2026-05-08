using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;

namespace ClearBank.DeveloperTest.Services;

public class PaymentService(
    IAccountDataStoreFactory dataStoreFactory,
    IPaymentValidatorFactory validatorFactory) : IPaymentService
{
    public MakePaymentResult MakePayment(MakePaymentRequest request)
    {
        var dataStore = dataStoreFactory.Create();
        var account = dataStore.GetAccount(request.DebtorAccountNumber);

        var validator = validatorFactory.GetValidator(request.PaymentScheme);
        var isValid = validator.IsValid(account, request);

        if (isValid)
        {
            account.Balance -= request.Amount;
            dataStore.UpdateAccount(account);
        }

        return new MakePaymentResult { Success = isValid };
    }
}
