### Test Description
In the 'PaymentService.cs' file you will find a method for making a payment. At a high level the steps for making a payment are:

 - Lookup the account the payment is being made from
 - Check the account is in a valid state to make the payment
 - Deduct the payment amount from the account's balance and update the account in the database
 
What we’d like you to do is refactor the code with the following things in mind:  
 - Adherence to SOLID principals
 - Testability  
 - Readability 

We’d also like you to add some unit tests to the ClearBank.DeveloperTest.Tests project to show how you would test the code that you’ve produced. The only specific ‘rules’ are:  

 - The solution should build.
 - The tests should all pass.
 - You should not change the method signature of the MakePayment method.

You are free to use any frameworks/NuGet packages that you see fit.  
 
You should plan to spend around 1 to 3 hours to complete the exercise.

## Refactoring Summary

The original `PaymentService.MakePayment` method was a single monolithic method that handled data store selection, payment validation for all schemes, balance deduction, and account persistence. The refactoring breaks these responsibilities apart while preserving the original behaviour.

- Extracted interfaces, removing tight coupling and enabling mocking.
- `PaymentService` no longer knows about configuration or concrete store types.
- Extracted payment validation into a strategy pattern using a keyed DI per scheme.
- `MakePayment` signature had to remain unchanged, otherwise an async approach would be more suitable.

## Design Decisions

- `IReadOnlySet<PaymentScheme>` instead of `[Flags]` enum for `AllowedPaymentSchemes`. Clearer than bitwise operations. Only affects the domain model, not any DB contract.
- `IConfiguration` instead of `ConfigurationManager.AppSettings`. Improves Testability and aligns better with modern practices.
- Previously, an invalid `DataStoreType` config (e.g. 'Bckup') would cause the app to use AccountDataStore. Now, it throws instead of silently defaulting.
- No early request validation in `MakePayment`. Out of scope for this refactor, but worth adding (e.g. amount > 0 before hitting the data store)
