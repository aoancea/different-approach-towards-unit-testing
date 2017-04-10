using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ragnar.Integration.InterestV3.Calculator.Contract;
using Ragnar.Integration.InterestV3.Calculator.Helpers;
using Ragnar.Integration.InterestV3.Model;
using Ragnar.Integration.InterestV3.Repository;
using SimpleInjector;
using System;

namespace Ragnar.Integration.UnitTesting.InterestV3.Calculator
{
    [TestClass]
    public class InterestCalculatorTest
    {
        private Bank bank;

        private Integration.InterestV3.Calculator.InterestCalculator interestCalculator;

        [TestInitialize]
        public void TestInitialize()
        {
            Container container = new Container();

            UnitTestBootstrapper.Register(container);

            interestCalculator = new Integration.InterestV3.Calculator.InterestCalculator(
                bankRepository: new BankRepository_Mock(() => bank),
                policyHelper: container.GetInstance<IPolicyHelper>(),
                taxHelper: container.GetInstance<ITaxHelper>(),
                interestHelper: container.GetInstance<IInterestHelper>(),
                interestRateHelper: container.GetInstance<InterestRateHelper>());
        }

        [TestMethod]
        public void ProjectDepositSummary_ApplyInterestRateOnly_ReturnProjectedSummary()
        {
            bank = ScenarioHelper.CreateBank(name: "BT");

            TaxSystem taxSystem = bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01));

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), amount: 100M);

            DepositProjectionSummary expectedSummary = ScenarioHelper.CreateDepositProjectionSummary(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), initialAmount: 100M);
            expectedSummary.AddInterest(asGross: 3M, asNet: 3M);
            expectedSummary.AddTax(asPercentage: 0M, asValue: 0M);

            DepositProjectionSummary actualSummary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            RagnarAssert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void ProjectDepositSummary_ApplyInterestRateAndTaxIfEqual_ReturnProjectedSummary()
        {
            bank = ScenarioHelper.CreateBank(name: "BT");

            bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01))
                .AddTaxPolicy(policyType: PolicyType.TaxPercentage, comparisonAction: ComparisonAction.Equal, comparisonValue: 480000M, taxValue: 0.16M, order: 0);

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), amount: 480000M);

            DepositProjectionSummary expectedSummary = ScenarioHelper.CreateDepositProjectionSummary(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), initialAmount: 480000M);
            expectedSummary.AddInterest(asGross: 14400M, asNet: 12096M);
            expectedSummary.AddTax(asPercentage: 0.16M, asValue: 2304M);

            DepositProjectionSummary actualSummary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            RagnarAssert.AreEqual(expectedSummary, actualSummary);
        }

        private class BankRepository_Mock : IBankRepository
        {
            private readonly Func<Bank> getterBank;

            public BankRepository_Mock(Func<Bank> getterBank)
            {
                this.getterBank = getterBank;
            }

            public Bank Detail(Guid bankId, Guid userId)
            {
                return getterBank();
            }
        }
    }
}
