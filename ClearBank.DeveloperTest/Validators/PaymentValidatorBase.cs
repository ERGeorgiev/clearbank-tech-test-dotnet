using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public abstract class PaymentValidatorBase(PaymentScheme scheme) : IPaymentValidator
{
    public virtual bool IsValid(Account account, MakePaymentRequest request)
    {
        return account != null
            && account.AllowedPaymentSchemes.Contains(scheme);
    }
}
