using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class BacsPaymentValidatorTests
{
    [Fact]
    public void Scheme_ReturnsBacs()
    {
        Assert.Equal(PaymentScheme.Bacs, new BacsPaymentValidator().Scheme);
    }
}
