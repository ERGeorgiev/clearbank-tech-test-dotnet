using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class BacsPaymentValidator : PaymentValidatorBase
{
    public override PaymentScheme Scheme => PaymentScheme.Bacs;
}
