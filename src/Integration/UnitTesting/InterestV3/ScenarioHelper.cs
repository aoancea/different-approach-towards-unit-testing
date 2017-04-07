using System;
using System.Linq;

namespace Ragnar.Integration.UnitTesting.InterestV3
{
    public static class ScenarioHelper
    {
        public static Guid userId = new Guid("b04e4eb0-f7b2-408d-ac79-94ad886f4ffa");
        public static Guid bankId = new Guid("18d1122e-67a3-4c08-82f3-53566fe89ac0");
        public static Guid depositId = new Guid("644ada56-32ef-496a-854d-3dd5e12b375c");

        public static Integration.InterestV3.Model.Bank CreateBank(Guid? id = null, string name = null)
        {
            return new Integration.InterestV3.Model.Bank()
            {
                Id = id ?? Guid.NewGuid(),
                Name = name ?? "Bank with no name!"
            };
        }

        public static Integration.InterestV3.Model.BankInterestRate AddInterestRate(this Integration.InterestV3.Model.Bank bank, DateTime startDate, decimal value)
        {
            Integration.InterestV3.Model.BankInterestRate bankInterestRate = new Integration.InterestV3.Model.BankInterestRate()
            {
                StartDate = startDate,
                Value = value
            };

            bank.Rates = (bank.Rates ?? new Integration.InterestV3.Model.BankInterestRate[0]).Concat(new[] { bankInterestRate }).ToArray();

            return bankInterestRate;
        }

        public static Integration.InterestV3.Model.BankAccount AddBankAccount(this Integration.InterestV3.Model.Bank bank, Guid? id)
        {
            Integration.InterestV3.Model.BankAccount bankAccount = new Integration.InterestV3.Model.BankAccount()
            {
                ID = id ?? Guid.NewGuid()
            };

            bank.BankAccount = bankAccount;

            return bankAccount;
        }

        public static Integration.InterestV3.Model.TaxSystem AddTaxSystem(this Integration.InterestV3.Model.Bank bank, DateTime effectiveDate)
        {
            Integration.InterestV3.Model.TaxSystem taxSystem = new Integration.InterestV3.Model.TaxSystem()
            {
                StartDate = effectiveDate,
                TaxPolicies = new Integration.InterestV3.Model.TaxPolicy[0]
            };

            bank.TaxSystem = (bank.TaxSystem ?? new System.Collections.Generic.Dictionary<DateTime, Integration.InterestV3.Model.TaxSystem>());
            bank.TaxSystem.Add(effectiveDate, taxSystem);

            return taxSystem;
        }

        public static Integration.InterestV3.Model.TaxPolicy AddTaxPolicy(
            this Integration.InterestV3.Model.TaxSystem taxSystem,
            Integration.InterestV3.Model.PolicyType policyType,
            Integration.InterestV3.Model.ComparisonAction comparisonAction,
            object comparisonValue,
            decimal? taxValue = null,
            int? order = null)
        {
            Integration.InterestV3.Model.TaxPolicy taxPolicy = new Integration.InterestV3.Model.TaxPolicy()
            {
                PolicyType = policyType,
                ComparisonAction = comparisonAction,
                ComparisonValue = comparisonValue,
                TaxValue = taxValue ?? decimal.Zero,
                Order = order ?? 0,
            };

            taxSystem.TaxPolicies = (taxSystem.TaxPolicies ?? new Integration.InterestV3.Model.TaxPolicy[0]).Concat(new[] { taxPolicy }).ToArray();

            return taxPolicy;
        }

        public static Integration.InterestV3.Model.Deposit AddDeposit(this Integration.InterestV3.Model.BankAccount bank, DateTime startDate, DateTime endDate, Guid? id = null, decimal? amount = null)
        {
            Integration.InterestV3.Model.Deposit deposit = new Integration.InterestV3.Model.Deposit()
            {
                ID = id ?? Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                Amount = amount ?? decimal.Zero
            };

            bank.Deposits = (bank.Deposits ?? new Integration.InterestV3.Model.Deposit[0]).Concat(new[] { deposit }).ToArray();

            return deposit;
        }
    }
}
