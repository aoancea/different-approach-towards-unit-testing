using System;
using System.Linq;

namespace Ragnar.Mock.UnitTesting.InterestV3
{
    public static class ScenarioHelper
    {
        public static Guid userId = new Guid("b04e4eb0-f7b2-408d-ac79-94ad886f4ffa");
        public static Guid bankId = new Guid("18d1122e-67a3-4c08-82f3-53566fe89ac0");
        public static Guid depositId = new Guid("644ada56-32ef-496a-854d-3dd5e12b375c");

        public static Mock.InterestV3.Model.Bank CreateBank(Guid? id = null, string name = null)
        {
            return new Mock.InterestV3.Model.Bank()
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Bank with no name!"
            };
        }

        public static Mock.InterestV3.Model.BankInterestRate AddInterestRate(this Mock.InterestV3.Model.Bank bank, DateTime startDate, decimal value)
        {
            Mock.InterestV3.Model.BankInterestRate bankInterestRate = new Mock.InterestV3.Model.BankInterestRate()
            {
                StartDate = startDate,
                Value = value
            };

            bank.Rates = (bank.Rates ?? new Mock.InterestV3.Model.BankInterestRate[0]).Concat(new[] { bankInterestRate }).ToArray();

            return bankInterestRate;
        }

        public static Mock.InterestV3.Model.BankAccount AddBankAccount(this Mock.InterestV3.Model.Bank bank, Guid? id)
        {
            Mock.InterestV3.Model.BankAccount bankAccount = new Mock.InterestV3.Model.BankAccount()
            {
                ID = id ?? Guid.NewGuid()
            };

            bank.BankAccount = bankAccount;

            return bankAccount;
        }

        public static Mock.InterestV3.Model.TaxSystem AddTaxSystem(this Mock.InterestV3.Model.Bank bank, DateTime effectiveDate)
        {
            Mock.InterestV3.Model.TaxSystem taxSystem = new Mock.InterestV3.Model.TaxSystem()
            {
                StartDate = effectiveDate,
                TaxPolicies = new Mock.InterestV3.Model.TaxPolicy[0]
            };

            bank.TaxSystem = (bank.TaxSystem ?? new System.Collections.Generic.Dictionary<DateTime, Mock.InterestV3.Model.TaxSystem>());
            bank.TaxSystem.Add(effectiveDate, taxSystem);

            return taxSystem;
        }

        public static Mock.InterestV3.Model.TaxPolicy AddTaxPolicy(
            this Mock.InterestV3.Model.TaxSystem taxSystem,
            Mock.InterestV3.Model.PolicyType policyType,
            Mock.InterestV3.Model.ComparisonAction comparisonAction,
            object comparisonValue,
            decimal? taxValue = null,
            int? order = null)
        {
            Mock.InterestV3.Model.TaxPolicy taxPolicy = new Mock.InterestV3.Model.TaxPolicy()
            {
                PolicyType = policyType,
                ComparisonAction = comparisonAction,
                ComparisonValue = comparisonValue,
                TaxValue = taxValue ?? decimal.Zero,
                Order = order ?? 0,
            };

            taxSystem.TaxPolicies = taxSystem.TaxPolicies ?? new Mock.InterestV3.Model.TaxPolicy[0];
            taxSystem.TaxPolicies = taxSystem.TaxPolicies.Concat(new[] { taxPolicy }).ToArray();

            return taxPolicy;
        }

        public static Mock.InterestV3.Model.Deposit AddDeposit(this Mock.InterestV3.Model.BankAccount bank, DateTime startDate, DateTime endDate, Guid? id = null, decimal? amount = null)
        {
            Mock.InterestV3.Model.Deposit deposit = new Mock.InterestV3.Model.Deposit()
            {
                ID = id ?? ScenarioHelper.depositId,
                StartDate = startDate,
                EndDate = endDate,
                Amount = amount ?? decimal.Zero
            };

            bank.Deposits = (bank.Deposits ?? new Mock.InterestV3.Model.Deposit[0]).Concat(new[] { deposit }).ToArray();

            return deposit;
        }

        public static Mock.InterestV3.Calculator.Helpers.PolicyCalculationContext CreatePolicyCalculationContext(Mock.InterestV3.Model.TaxPolicy[] taxPolicies, Mock.InterestV3.Model.Deposit deposit)
        {
            return new Mock.InterestV3.Calculator.Helpers.PolicyCalculationContext()
            {
                TaxPolicies = taxPolicies,
                Deposit = deposit
            };
        }


        public static Mock.InterestV3.Calculator.Contract.DepositProjectionSummary CreateDepositProjectionSummary(Guid? depositId = null, DateTime? startDate = null, DateTime? endDate = null, decimal? initialAmount = null)
        {
            Mock.InterestV3.Calculator.Contract.DepositProjectionSummary summary = new Mock.InterestV3.Calculator.Contract.DepositProjectionSummary()
            {
                DepositID = depositId ?? ScenarioHelper.depositId,
                StartDate = startDate ?? DateTime.MinValue,
                EndDate = endDate ?? DateTime.MinValue,
                InitialAmount = initialAmount ?? decimal.Zero
            };

            return summary;
        }

        public static Mock.InterestV3.Calculator.Contract.Interest AddInterest(this Mock.InterestV3.Calculator.Contract.DepositProjectionSummary summary, decimal? asGross = null, decimal? asNet = null)
        {
            Mock.InterestV3.Calculator.Contract.Interest interest = new Mock.InterestV3.Calculator.Contract.Interest()
            {
                AsGross = asGross ?? decimal.Zero,
                AsNet = asNet ?? decimal.Zero
            };

            summary.Interest = interest;

            return interest;
        }

        public static Mock.InterestV3.Calculator.Contract.Tax AddTax(this Mock.InterestV3.Calculator.Contract.DepositProjectionSummary summary, decimal? asPercentage = null, decimal? asValue = null)
        {
            Mock.InterestV3.Calculator.Contract.Tax tax = new Mock.InterestV3.Calculator.Contract.Tax()
            {
                AsPercentage = asPercentage ?? decimal.Zero,
                AsValue = asValue ?? decimal.Zero
            };

            summary.Tax = tax;

            return tax;
        }
    }
}