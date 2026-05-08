using System.Collections.Generic;

namespace ClearBank.DeveloperTest.Types;

public class Account
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public IReadOnlySet<PaymentScheme> AllowedPaymentSchemes { get; set; }
}