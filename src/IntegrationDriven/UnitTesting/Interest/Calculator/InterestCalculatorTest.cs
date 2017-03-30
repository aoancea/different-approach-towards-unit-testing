using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleInjector;
using System;

namespace Ragnar.IntegrationDriven.UnitTesting.Interest.Calculator
{
    [TestClass]
    public class InterestCalculatorTest
    {
        private Mock<IntegrationDriven.Interest.Repository.IBankRepository> bankRepositoryMock;

        private Container container;

        private IntegrationDriven.Interest.Calculator.InterestCalculator interestCalculator;

        [TestInitialize]
        public void TestInitialize()
        {
            bankRepositoryMock = new Mock<IntegrationDriven.Interest.Repository.IBankRepository>(MockBehavior.Strict);

            container = new Container();

            UnitTestBootstrapper.Register(container);

            interestCalculator = new IntegrationDriven.Interest.Calculator.InterestCalculator(
                bankRepository: bankRepositoryMock.Object,
                policyHelper: container.GetInstance<IntegrationDriven.Interest.Calculator.Helpers.IPolicyHelper>(),
                taxHelper: container.GetInstance<IntegrationDriven.Interest.Calculator.Helpers.ITaxHelper>(),
                interestHelper: container.GetInstance<IntegrationDriven.Interest.Calculator.Helpers.IInterestHelper>(),
                comparisonHelper: container.GetInstance<IntegrationDriven.Interest.Calculator.Helpers.IComparisonHelper>());
        }

        [TestMethod]
        public void ProjectDepositSummary_ReturnSummary()
        {
            IntegrationDriven.Interest.Model.Bank bank = ScenarioHelper.CreateBank(id: Guid.NewGuid(), name: "BT");

            IntegrationDriven.Interest.Model.TaxSystem taxSystem = bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01));

            IntegrationDriven.Interest.Model.BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            IntegrationDriven.Interest.Model.BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            IntegrationDriven.Interest.Model.Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), id: Guid.NewGuid(), amount: 100);

            bankRepositoryMock.Setup(x => x.Detail(bank.Id, ScenarioHelper.userId)).Returns(bank);

            IntegrationDriven.Interest.Calculator.Contract.DepositProjectionSummary summary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            Assert.AreEqual(2.9917808219178082191780828400M, summary.Interest.AsGross);
            Assert.AreEqual(2.9917808219178082191780828400M, summary.Interest.AsNet);
        }
    }
}
