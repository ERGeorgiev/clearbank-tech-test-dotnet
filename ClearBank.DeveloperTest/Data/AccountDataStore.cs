using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data;

internal class AccountDataStore : IAccountDataStore
{
    public Account GetAccount(string accountNumber)
    {
        // Access database to retrieve account, code removed for brevity
        return new Account() { AccountNumber = accountNumber, AllowedPaymentSchemes = new HashSet<PaymentScheme>() };
    }

    public void UpdateAccount(Account account)
    {
        // Update account in database, code removed for brevity
    }
}
