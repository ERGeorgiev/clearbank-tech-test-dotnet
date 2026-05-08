using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public interface IPaymentValidator
{
    bool IsValid(Account account, MakePaymentRequest request);
}
