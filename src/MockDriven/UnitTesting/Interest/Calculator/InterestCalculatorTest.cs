using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ragnar.MockDriven.Interest.Calculator.Contract;
using Ragnar.MockDriven.Interest.Calculator.Helpers;
using Ragnar.MockDriven.Interest.Model;
using Ragnar.MockDriven.Interest.Repository;
using System;

namespace Ragnar.MockDriven.UnitTesting.Interest.Calculator
{
    [TestClass]
    public class InterestCalculatorTest
    {
        private Mock<IBankRepository> bankRepositoryMock;
        private Mock<IPolicyHelper> policyHelperMock;
        private Mock<ITaxHelper> taxHelperMock;
        private Mock<IInterestHelper> interestHelperMock;
        private Mock<IComparisonHelper> comparisonHelperMock;

        private MockDriven.Interest.Calculator.InterestCalculator interestCalculator;

        [TestInitialize]
        public void TestInitialize()
        {
            bankRepositoryMock = new Mock<IBankRepository>(MockBehavior.Strict);
            policyHelperMock = new Mock<IPolicyHelper>(MockBehavior.Strict);
            taxHelperMock = new Mock<ITaxHelper>(MockBehavior.Strict);
            interestHelperMock = new Mock<IInterestHelper>(MockBehavior.Strict);
            comparisonHelperMock = new Mock<IComparisonHelper>(MockBehavior.Strict);

            interestCalculator = new MockDriven.Interest.Calculator.InterestCalculator(
                bankRepository: bankRepositoryMock.Object,
                policyHelper: policyHelperMock.Object,
                taxHelper: taxHelperMock.Object,
                interestHelper: interestHelperMock.Object,
                comparisonHelper: comparisonHelperMock.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            bankRepositoryMock.VerifyAll();
            policyHelperMock.VerifyAll();
            taxHelperMock.VerifyAll();
            interestHelperMock.VerifyAll();
            comparisonHelperMock.VerifyAll();
        }

        [TestMethod]
        public void InterestCalculatorTest_ProjectDepositSummary_ApplyInterestRateOnly_ReturnProjectedSummary()
        {
            Bank bank = ScenarioHelper.CreateBank(id: Guid.NewGuid(), name: "BT");

            TaxSystem taxSystem = bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01));

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), id: Guid.NewGuid(), amount: 100);

            bankRepositoryMock.Setup(x => x.Detail(bank.Id, ScenarioHelper.userId)).Returns(bank);
            taxHelperMock.Setup(x => x.Tax(It.IsAny<decimal>(), 0M)).Returns(new Tax() { AsPercentage = 0M, AsValue = 0M });
            interestHelperMock.Setup(x => x.Interest(It.IsAny<decimal>(), 0M)).Returns(new MockDriven.Interest.Calculator.Contract.Interest() { AsGross = 3M, AsNet = 3M });

            DepositProjectionSummary summary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            Assert.AreEqual(deposit.ID, summary.DepositID);
            Assert.AreEqual(deposit.StartDate, summary.StartDate);
            Assert.AreEqual(deposit.EndDate, summary.EndDate);
            Assert.AreEqual(deposit.Amount, summary.InitialAmount);
            Assert.AreEqual(3M, summary.Interest.AsGross); // the right value is actually 2.9917808219178082191780828400M
            Assert.AreEqual(3M, summary.Interest.AsNet); // the right value is actually 2.9917808219178082191780828400M
            Assert.AreEqual(0M, summary.Tax.AsPercentage);
            Assert.AreEqual(0M, summary.Tax.AsValue);
        }

        [TestMethod]
        public void InterestCalculatorTest_ProjectDepositSummary_ApplyInterestRateAndTaxIfEqual_ReturnProjectedSummary()
        {
            Bank bank = ScenarioHelper.CreateBank(id: Guid.NewGuid(), name: "BT");

            bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01))
                .AddTaxPolicy(policyType: PolicyType.TaxPercentage, comparisonAction: ComparisonAction.Equal, comparisonValue: 480000M, taxValue: 0.16M, order: 0);

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), id: Guid.NewGuid(), amount: 480000M);

            bankRepositoryMock.Setup(x => x.Detail(bank.Id, ScenarioHelper.userId)).Returns(bank);
            taxHelperMock.Setup(x => x.Tax(14360.547945205479452054797632M, 0.16M)).Returns(new Tax() { AsPercentage = 0.16M, AsValue = 2304M });
            interestHelperMock.Setup(x => x.Interest(14360.547945205479452054797632M, 0.16M)).Returns(new MockDriven.Interest.Calculator.Contract.Interest() { AsGross = 14400M, AsNet = 12096M });
            comparisonHelperMock.Setup(x => x.Compare(ComparisonAction.Equal, 480000M, 480000M)).Returns(true);

            DepositProjectionSummary summary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            Assert.AreEqual(deposit.ID, summary.DepositID);
            Assert.AreEqual(deposit.StartDate, summary.StartDate);
            Assert.AreEqual(deposit.EndDate, summary.EndDate);
            Assert.AreEqual(deposit.Amount, summary.InitialAmount);
            Assert.AreEqual(14400M, summary.Interest.AsGross);
            Assert.AreEqual(12096M, summary.Interest.AsNet);
            Assert.AreEqual(0.16M, summary.Tax.AsPercentage);
            Assert.AreEqual(2304M, summary.Tax.AsValue);
        }
    }
}