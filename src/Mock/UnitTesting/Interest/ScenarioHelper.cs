using System;
using System.Linq;

namespace Ragnar.Mock.UnitTesting.Interest
{
    public static class ScenarioHelper
    {
        public static Guid userId = new Guid("b04e4eb0-f7b2-408d-ac79-94ad886f4ffa");
        public static Guid bankId = new Guid("18d1122e-67a3-4c08-82f3-53566fe89ac0");
        public static Guid depositId = new Guid("644ada56-32ef-496a-854d-3dd5e12b375c");

        public static Mock.Interest.Model.Bank CreateBank(Guid? id = null, string name = null)
        {
            return new Mock.Interest.Model.Bank()
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Bank with no name!"
            };
        }

        public static Mock.Interest.Model.BankInterestRate AddInterestRate(this Mock.Interest.Model.Bank bank, DateTime startDate, decimal value)
        {
            Mock.Interest.Model.BankInterestRate bankInterestRate = new Mock.Interest.Model.BankInterestRate()
            {
                StartDate = startDate,
                Value = value
            };

            bank.Rates = (bank.Rates ?? new Mock.Interest.Model.BankInterestRate[0]).Concat(new[] { bankInterestRate }).ToArray();

            return bankInterestRate;
        }

        public static Mock.Interest.Model.BankAccount AddBankAccount(this Mock.Interest.Model.Bank bank, Guid? id)
        {
            Mock.Interest.Model.BankAccount bankAccount = new Mock.Interest.Model.BankAccount()
            {
                ID = id ?? Guid.NewGuid()
            };

            bank.BankAccount = bankAccount;

            return bankAccount;
        }

        public static Mock.Interest.Model.TaxSystem AddTaxSystem(this Mock.Interest.Model.Bank bank, DateTime effectiveDate)
        {
            Mock.Interest.Model.TaxSystem taxSystem = new Mock.Interest.Model.TaxSystem()
            {
                StartDate = effectiveDate,
                TaxPolicies = new Mock.Interest.Model.TaxPolicy[0]
            };

            bank.TaxSystem = (bank.TaxSystem ?? new System.Collections.Generic.Dictionary<DateTime, Mock.Interest.Model.TaxSystem>());
            bank.TaxSystem.Add(effectiveDate, taxSystem);

            return taxSystem;
        }

        public static Mock.Interest.Model.TaxPolicy AddTaxPolicy(
            this Mock.Interest.Model.TaxSystem taxSystem,
            Mock.Interest.Model.PolicyType policyType,
            Mock.Interest.Model.ComparisonAction comparisonAction,
            object comparisonValue,
            decimal? taxValue = null,
            int? order = null)
        {
            Mock.Interest.Model.TaxPolicy taxPolicy = new Mock.Interest.Model.TaxPolicy()
            {
                PolicyType = policyType,
                ComparisonAction = comparisonAction,
                ComparisonValue = comparisonValue,
                TaxValue = taxValue ?? decimal.Zero,
                Order = order ?? 0,
            };

            taxSystem.TaxPolicies = taxSystem.TaxPolicies ?? new Mock.Interest.Model.TaxPolicy[0];
            taxSystem.TaxPolicies = taxSystem.TaxPolicies.Concat(new[] { taxPolicy }).ToArray();

            return taxPolicy;
        }

        public static Mock.Interest.Model.Deposit AddDeposit(this Mock.Interest.Model.BankAccount bank, DateTime startDate, DateTime endDate, Guid? id = null, decimal? amount = null)
        {
            Mock.Interest.Model.Deposit deposit = new Mock.Interest.Model.Deposit()
            {
                ID = id ?? Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                Amount = amount ?? decimal.Zero
            };

            bank.Deposits = (bank.Deposits ?? new Mock.Interest.Model.Deposit[0]).Concat(new[] { deposit }).ToArray();

            return deposit;
        }
    }
}