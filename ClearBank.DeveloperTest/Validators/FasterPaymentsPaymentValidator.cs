using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class FasterPaymentsPaymentValidator() : PaymentValidatorBase(PaymentScheme.FasterPayments)
{
    public override bool IsValid(Account account, MakePaymentRequest request)
    {
        return base.IsValid(account, request)
            && account.Balance >= request.Amount;
    }
}
