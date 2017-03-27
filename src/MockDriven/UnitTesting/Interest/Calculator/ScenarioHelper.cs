using System;
using System.Linq;

namespace Ragnar.MockDriven.UnitTesting.Interest.Calculator
{
    public static class ScenarioHelper
    {
        public static Guid userId = new Guid("b04e4eb0-f7b2-408d-ac79-94ad886f4ffa");
        public static Guid bankId = new Guid("18d1122e-67a3-4c08-82f3-53566fe89ac0");
        public static Guid depositId = new Guid("644ada56-32ef-496a-854d-3dd5e12b375c");

        public static MockDriven.Interest.Model.Bank CreateBank(Guid? id = null, string name = null)
        {
            return new MockDriven.Interest.Model.Bank()
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Bank with no name!"
            };
        }

        public static MockDriven.Interest.Model.BankInterestRate AddInterestRate(this MockDriven.Interest.Model.Bank bank, DateTime startDate, decimal value)
        {
            MockDriven.Interest.Model.BankInterestRate bankInterestRate = new MockDriven.Interest.Model.BankInterestRate()
            {
                StartDate = startDate,
                Value = value
            };

            bank.Rates = (bank.Rates ?? new MockDriven.Interest.Model.BankInterestRate[0]).Concat(new[] { bankInterestRate }).ToArray();

            return bankInterestRate;
        }

        public static MockDriven.Interest.Model.BankAccount AddBankAccount(this MockDriven.Interest.Model.Bank bank, Guid? id)
        {
            MockDriven.Interest.Model.BankAccount bankAccount = new MockDriven.Interest.Model.BankAccount()
            {
                ID = id ?? Guid.NewGuid()
            };

            bank.BankAccount = bankAccount;

            return bankAccount;
        }

        public static MockDriven.Interest.Model.TaxSystem AddTaxSystem(this MockDriven.Interest.Model.Bank bank, DateTime effectiveDate)
        {
            MockDriven.Interest.Model.TaxSystem taxSystem = new MockDriven.Interest.Model.TaxSystem()
            {
                StartDate = effectiveDate,
                TaxPolicies = new MockDriven.Interest.Model.TaxPolicy[0]
            };

            bank.TaxSystem = (bank.TaxSystem ?? new System.Collections.Generic.Dictionary<DateTime, MockDriven.Interest.Model.TaxSystem>());
            bank.TaxSystem.Add(effectiveDate, taxSystem);

            return taxSystem;
        }

        public static MockDriven.Interest.Model.Deposit AddDeposit(this MockDriven.Interest.Model.BankAccount bank, DateTime startDate, DateTime endDate, Guid? id = null, decimal? amount = null)
        {
            MockDriven.Interest.Model.Deposit deposit = new MockDriven.Interest.Model.Deposit()
            {
                ID = id ?? Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                Amount = amount ?? decimal.Zero
            };

            bank.Deposits = (bank.Deposits ?? new MockDriven.Interest.Model.Deposit[0]).Concat(new[] { deposit }).ToArray();

            return deposit;
        }
    }
}