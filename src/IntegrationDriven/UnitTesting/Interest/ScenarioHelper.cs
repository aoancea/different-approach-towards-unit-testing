using System;
using System.Linq;

namespace Ragnar.IntegrationDriven.UnitTesting.Interest
{
    public static class ScenarioHelper
    {
        public static Guid userId = new Guid("b04e4eb0-f7b2-408d-ac79-94ad886f4ffa");
        public static Guid bankId = new Guid("18d1122e-67a3-4c08-82f3-53566fe89ac0");
        public static Guid depositId = new Guid("644ada56-32ef-496a-854d-3dd5e12b375c");

        public static IntegrationDriven.Interest.Model.Bank CreateBank(Guid? id = null, string name = null)
        {
            return new IntegrationDriven.Interest.Model.Bank()
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Bank with no name!"
            };
        }

        public static IntegrationDriven.Interest.Model.BankInterestRate AddInterestRate(this IntegrationDriven.Interest.Model.Bank bank, DateTime startDate, decimal value)
        {
            IntegrationDriven.Interest.Model.BankInterestRate bankInterestRate = new IntegrationDriven.Interest.Model.BankInterestRate()
            {
                StartDate = startDate,
                Value = value
            };

            bank.Rates = (bank.Rates ?? new IntegrationDriven.Interest.Model.BankInterestRate[0]).Concat(new[] { bankInterestRate }).ToArray();

            return bankInterestRate;
        }

        public static IntegrationDriven.Interest.Model.BankAccount AddBankAccount(this IntegrationDriven.Interest.Model.Bank bank, Guid? id)
        {
            IntegrationDriven.Interest.Model.BankAccount bankAccount = new IntegrationDriven.Interest.Model.BankAccount()
            {
                ID = id ?? Guid.NewGuid()
            };

            bank.BankAccount = bankAccount;

            return bankAccount;
        }

        public static IntegrationDriven.Interest.Model.TaxSystem AddTaxSystem(this IntegrationDriven.Interest.Model.Bank bank, DateTime effectiveDate)
        {
            IntegrationDriven.Interest.Model.TaxSystem taxSystem = new IntegrationDriven.Interest.Model.TaxSystem()
            {
                StartDate = effectiveDate,
                TaxPolicies = new IntegrationDriven.Interest.Model.TaxPolicy[0]
            };

            bank.TaxSystem = (bank.TaxSystem ?? new System.Collections.Generic.Dictionary<DateTime, IntegrationDriven.Interest.Model.TaxSystem>());
            bank.TaxSystem.Add(effectiveDate, taxSystem);

            return taxSystem;
        }

        public static IntegrationDriven.Interest.Model.TaxPolicy AddTaxPolicy(
            this IntegrationDriven.Interest.Model.TaxSystem taxSystem,
            IntegrationDriven.Interest.Model.PolicyType policyType,
            IntegrationDriven.Interest.Model.ComparisonAction comparisonAction,
            object comparisonValue,
            decimal? taxValue = null,
            int? order = null)
        {
            IntegrationDriven.Interest.Model.TaxPolicy taxPolicy = new IntegrationDriven.Interest.Model.TaxPolicy()
            {
                PolicyType = policyType,
                ComparisonAction = comparisonAction,
                ComparisonValue = comparisonValue,
                TaxValue = taxValue ?? decimal.Zero,
                Order = order ?? 0,
            };

            taxSystem.TaxPolicies = taxSystem.TaxPolicies ?? new IntegrationDriven.Interest.Model.TaxPolicy[0];
            taxSystem.TaxPolicies = taxSystem.TaxPolicies.Concat(new[] { taxPolicy }).ToArray();

            return taxPolicy;
        }

        public static IntegrationDriven.Interest.Model.Deposit AddDeposit(this IntegrationDriven.Interest.Model.BankAccount bank, DateTime startDate, DateTime endDate, Guid? id = null, decimal? amount = null)
        {
            IntegrationDriven.Interest.Model.Deposit deposit = new IntegrationDriven.Interest.Model.Deposit()
            {
                ID = id ?? Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                Amount = amount ?? decimal.Zero
            };

            bank.Deposits = (bank.Deposits ?? new IntegrationDriven.Interest.Model.Deposit[0]).Concat(new[] { deposit }).ToArray();

            return deposit;
        }
    }
}
