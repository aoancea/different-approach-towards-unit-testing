﻿using System;
using System.Linq;

namespace Ragnar.Mock.UnitTesting.InterestV2
{
    public static class ScenarioHelper
    {
        public static Guid userId = new Guid("b04e4eb0-f7b2-408d-ac79-94ad886f4ffa");
        public static Guid bankId = new Guid("18d1122e-67a3-4c08-82f3-53566fe89ac0");
        public static Guid depositId = new Guid("644ada56-32ef-496a-854d-3dd5e12b375c");

        public static Mock.InterestV2.Model.Bank CreateBank(Guid? id = null, string name = null)
        {
            return new Mock.InterestV2.Model.Bank()
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Bank with no name!"
            };
        }

        public static Mock.InterestV2.Model.BankInterestRate AddInterestRate(this Mock.InterestV2.Model.Bank bank, DateTime startDate, decimal value)
        {
            Mock.InterestV2.Model.BankInterestRate bankInterestRate = new Mock.InterestV2.Model.BankInterestRate()
            {
                StartDate = startDate,
                Value = value
            };

            bank.Rates = (bank.Rates ?? new Mock.InterestV2.Model.BankInterestRate[0]).Concat(new[] { bankInterestRate }).ToArray();

            return bankInterestRate;
        }

        public static Mock.InterestV2.Model.BankAccount AddBankAccount(this Mock.InterestV2.Model.Bank bank, Guid? id)
        {
            Mock.InterestV2.Model.BankAccount bankAccount = new Mock.InterestV2.Model.BankAccount()
            {
                ID = id ?? Guid.NewGuid()
            };

            bank.BankAccount = bankAccount;

            return bankAccount;
        }

        public static Mock.InterestV2.Model.TaxSystem AddTaxSystem(this Mock.InterestV2.Model.Bank bank, DateTime effectiveDate)
        {
            Mock.InterestV2.Model.TaxSystem taxSystem = new Mock.InterestV2.Model.TaxSystem()
            {
                StartDate = effectiveDate,
                TaxPolicies = new Mock.InterestV2.Model.TaxPolicy[0]
            };

            bank.TaxSystem = (bank.TaxSystem ?? new System.Collections.Generic.Dictionary<DateTime, Mock.InterestV2.Model.TaxSystem>());
            bank.TaxSystem.Add(effectiveDate, taxSystem);

            return taxSystem;
        }

        public static Mock.InterestV2.Model.TaxPolicy AddTaxPolicy(
            this Mock.InterestV2.Model.TaxSystem taxSystem,
            Mock.InterestV2.Model.PolicyType policyType,
            Mock.InterestV2.Model.ComparisonAction comparisonAction,
            object comparisonValue,
            decimal? taxValue = null,
            int? order = null)
        {
            Mock.InterestV2.Model.TaxPolicy taxPolicy = new Mock.InterestV2.Model.TaxPolicy()
            {
                PolicyType = policyType,
                ComparisonAction = comparisonAction,
                ComparisonValue = comparisonValue,
                TaxValue = taxValue ?? decimal.Zero,
                Order = order ?? 0,
            };

            taxSystem.TaxPolicies = taxSystem.TaxPolicies ?? new Mock.InterestV2.Model.TaxPolicy[0];
            taxSystem.TaxPolicies = taxSystem.TaxPolicies.Concat(new[] { taxPolicy }).ToArray();

            return taxPolicy;
        }

        public static Mock.InterestV2.Model.Deposit AddDeposit(this Mock.InterestV2.Model.BankAccount bank, DateTime startDate, DateTime endDate, Guid? id = null, decimal? amount = null)
        {
            Mock.InterestV2.Model.Deposit deposit = new Mock.InterestV2.Model.Deposit()
            {
                ID = id ?? Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                Amount = amount ?? decimal.Zero
            };

            bank.Deposits = (bank.Deposits ?? new Mock.InterestV2.Model.Deposit[0]).Concat(new[] { deposit }).ToArray();

            return deposit;
        }

        public static Mock.InterestV2.Calculator.Helpers.PolicyCalculationContext CreatePolicyCalculationContext(Mock.InterestV2.Model.TaxPolicy[] taxPolicies, Mock.InterestV2.Model.Deposit deposit)
        {
            return new Mock.InterestV2.Calculator.Helpers.PolicyCalculationContext()
            {
                TaxPolicies = taxPolicies,
                Deposit = deposit
            };
        }
    }
}