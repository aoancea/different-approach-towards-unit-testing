using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ragnar.Integration.Interest.Calculator.Contract;
using Ragnar.Integration.Interest.Calculator.Helpers;
using Ragnar.Integration.Interest.Model;
using Ragnar.Integration.Interest.Repository;
using SimpleInjector;
using System;

namespace Ragnar.Integration.UnitTesting.Interest.Calculator
{
    [TestClass]
    public class InterestCalculatorTest
    {
        private Mock<IBankRepository> bankRepositoryMock;

        private Container container;

        private Integration.Interest.Calculator.InterestCalculator interestCalculator;

        [TestInitialize]
        public void TestInitialize()
        {
            bankRepositoryMock = new Mock<IBankRepository>(MockBehavior.Strict);

            container = new Container();

            UnitTestBootstrapper.Register(container);

            interestCalculator = new Integration.Interest.Calculator.InterestCalculator(
                bankRepository: bankRepositoryMock.Object,
                policyHelper: container.GetInstance<IPolicyHelper>(),
                taxHelper: container.GetInstance<ITaxHelper>(),
                interestHelper: container.GetInstance<IInterestHelper>(),
                comparisonHelper: container.GetInstance<IComparisonHelper>());
        }

        [TestMethod]
        public void ProjectDepositSummary_ApplyInterestRateOnly_ReturnProjectedSummary()
        {
            Bank bank = ScenarioHelper.CreateBank(id: Guid.NewGuid(), name: "BT");

            TaxSystem taxSystem = bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01));

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), id: Guid.NewGuid(), amount: 100);

            SetupMocks(bank);

            DepositProjectionSummary summary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            Assert.AreEqual(deposit.ID, summary.DepositID);
            Assert.AreEqual(deposit.StartDate, summary.StartDate);
            Assert.AreEqual(deposit.EndDate, summary.EndDate);
            Assert.AreEqual(deposit.Amount, summary.InitialAmount);
            Assert.AreEqual(2.9917808219178082191780828400M, summary.Interest.AsGross); // should be 3M
            Assert.AreEqual(2.9917808219178082191780828400M, summary.Interest.AsNet); // should be 3M
            Assert.AreEqual(0M, summary.Tax.AsPercentage);
            Assert.AreEqual(0M, summary.Tax.AsValue);
        }

        [TestMethod]
        public void ProjectDepositSummary_ApplyInterestRateAndTaxIfEqual_ReturnProjectedSummary()
        {
            Bank bank = ScenarioHelper.CreateBank(id: Guid.NewGuid(), name: "BT");

            bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01))
                .AddTaxPolicy(policyType: PolicyType.TaxPercentage, comparisonAction: ComparisonAction.Equal, comparisonValue: 480000M, taxValue: 0.16M, order: 0);

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), id: Guid.NewGuid(), amount: 480000M);

            SetupMocks(bank);

            DepositProjectionSummary summary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            Assert.AreEqual(deposit.ID, summary.DepositID);
            Assert.AreEqual(deposit.StartDate, summary.StartDate);
            Assert.AreEqual(deposit.EndDate, summary.EndDate);
            Assert.AreEqual(deposit.Amount, summary.InitialAmount);
            Assert.AreEqual(14360.547945205479452054797632M, summary.Interest.AsGross); // should be 14400M
            Assert.AreEqual(12062.860273972602739726030011M, summary.Interest.AsNet); // should be 12096M
            Assert.AreEqual(0.16M, summary.Tax.AsPercentage);
            Assert.AreEqual(2297.6876712328767123287676211M, summary.Tax.AsValue); // should be 2304M
        }

        private void SetupMocks(Bank bank)
        {
            bankRepositoryMock.Setup(x => x.Detail(bank.Id, ScenarioHelper.userId)).Returns(bank);
        }
    }
}
