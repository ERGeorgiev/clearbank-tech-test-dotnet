using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public abstract class PaymentValidatorBase : IPaymentValidator
{
    public abstract PaymentScheme Scheme { get; }

    public virtual bool IsValid(Account account, MakePaymentRequest request)
    {
        return account != null
            && account.AllowedPaymentSchemes.Contains(Scheme);
    }
}
