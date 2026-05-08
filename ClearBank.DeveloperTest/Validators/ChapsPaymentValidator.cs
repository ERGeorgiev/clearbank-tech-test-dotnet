using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class ChapsPaymentValidator() : PaymentValidatorBase(PaymentScheme.Chaps)
{
    public override bool IsValid(Account account, MakePaymentRequest request)
    {
        return base.IsValid(account, request)
            && account.Status == AccountStatus.Live;
    }
}
