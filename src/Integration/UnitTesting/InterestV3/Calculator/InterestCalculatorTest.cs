using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        private Mock<IBankRepository> bankRepositoryMock;

        private Container container;

        private Integration.InterestV3.Calculator.InterestCalculator interestCalculator;

        [TestInitialize]
        public void TestInitialize()
        {
            bankRepositoryMock = new Mock<IBankRepository>(MockBehavior.Strict);

            container = new Container();

            UnitTestBootstrapper.Register(container);

            interestCalculator = new Integration.InterestV3.Calculator.InterestCalculator(
                bankRepository: bankRepositoryMock.Object,
                policyHelper: container.GetInstance<IPolicyHelper>(),
                taxHelper: container.GetInstance<ITaxHelper>(),
                interestHelper: container.GetInstance<IInterestHelper>(),
                interestRateHelper: container.GetInstance<InterestRateHelper>());
        }

        [TestMethod]
        public void ProjectDepositSummary_ApplyInterestRateOnly_ReturnProjectedSummary()
        {
            Bank bank = ScenarioHelper.CreateBank(name: "BT");

            TaxSystem taxSystem = bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01));

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), amount: 100M);

            DepositProjectionSummary expectedSummary = ScenarioHelper.CreateDepositProjectionSummary(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), initialAmount: 100M);
            expectedSummary.AddInterest(asGross: 3M, asNet: 3M);
            expectedSummary.AddTax(asPercentage: 0M, asValue: 0M);

            SetupMocks(bank);

            DepositProjectionSummary actualSummary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            RagnarAssert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void ProjectDepositSummary_ApplyInterestRateAndTaxIfEqual_ReturnProjectedSummary()
        {
            Bank bank = ScenarioHelper.CreateBank(name: "BT");

            bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01))
                .AddTaxPolicy(policyType: PolicyType.TaxPercentage, comparisonAction: ComparisonAction.Equal, comparisonValue: 480000M, taxValue: 0.16M, order: 0);

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), amount: 480000M);

            DepositProjectionSummary expectedSummary = ScenarioHelper.CreateDepositProjectionSummary(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), initialAmount: 480000M);
            expectedSummary.AddInterest(asGross: 14400M, asNet: 12096M);
            expectedSummary.AddTax(asPercentage: 0.16M, asValue: 2304M);

            SetupMocks(bank);

            DepositProjectionSummary actualSummary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            RagnarAssert.AreEqual(expectedSummary, actualSummary);
        }

        private void SetupMocks(Bank bank)
        {
            bankRepositoryMock.Setup(x => x.Detail(bank.Id, ScenarioHelper.userId)).Returns(bank);
        }
    }
}
