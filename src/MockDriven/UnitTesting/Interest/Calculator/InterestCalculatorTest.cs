using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ragnar.MockDriven.UnitTesting.Interest.Calculator
{
    [TestClass]
    public class InterestCalculatorTest
    {
        private Mock<MockDriven.Interest.Repository.IBankRepository> bankRepositoryMock;
        private Mock<MockDriven.Interest.Calculator.Helpers.IPolicyHelper> policyHelperMock;
        private Mock<MockDriven.Interest.Calculator.Helpers.ITaxHelper> taxHelperMock;
        private Mock<MockDriven.Interest.Calculator.Helpers.IInterestHelper> interestHelperMock;
        private Mock<MockDriven.Interest.Calculator.Helpers.IComparisonHelper> comparisonHelperMock;

        private MockDriven.Interest.Calculator.InterestCalculator interestCalculator;

        [TestInitialize]
        public void TestInitialize()
        {
            bankRepositoryMock = new Mock<MockDriven.Interest.Repository.IBankRepository>(MockBehavior.Strict);
            policyHelperMock = new Mock<MockDriven.Interest.Calculator.Helpers.IPolicyHelper>(MockBehavior.Strict);
            taxHelperMock = new Mock<MockDriven.Interest.Calculator.Helpers.ITaxHelper>(MockBehavior.Strict);
            interestHelperMock = new Mock<MockDriven.Interest.Calculator.Helpers.IInterestHelper>(MockBehavior.Strict);
            comparisonHelperMock = new Mock<MockDriven.Interest.Calculator.Helpers.IComparisonHelper>(MockBehavior.Strict);

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
        public void ProjectDepositSummary_ReturnSummary()
        {
            MockDriven.Interest.Model.Bank bank = ScenarioHelper.CreateBank(id: Guid.NewGuid(), name: "BT");

            MockDriven.Interest.Model.TaxSystem taxSystem = bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01));

            MockDriven.Interest.Model.BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            MockDriven.Interest.Model.BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            MockDriven.Interest.Model.Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), id: Guid.NewGuid(), amount: 100);

            bankRepositoryMock.Setup(x => x.Detail(bank.Id, ScenarioHelper.userId)).Returns(bank);
            taxHelperMock.Setup(x => x.Tax(It.IsAny<decimal>(), 0M)).Returns(new MockDriven.Interest.Calculator.Contract.Tax() { AsPercentage = 0M, AsValue = 0M });
            interestHelperMock.Setup(x => x.Interest(It.IsAny<decimal>(), 0M)).Returns(new MockDriven.Interest.Calculator.Contract.Interest() { AsGross = 3M, AsNet = 3M });

            MockDriven.Interest.Calculator.Contract.DepositProjectionSummary summary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            Assert.AreEqual(3M, summary.Interest.AsGross); // the right value is actually 2.9917808219178082191780828400M
            Assert.AreEqual(3M, summary.Interest.AsNet); // the right value is actually 2.9917808219178082191780828400M
        }
    }
}