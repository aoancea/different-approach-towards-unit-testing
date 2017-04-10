using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ragnar.Mock.InterestV3.Calculator.Contract;
using Ragnar.Mock.InterestV3.Calculator.Helpers;
using Ragnar.Mock.InterestV3.Model;
using Ragnar.Mock.InterestV3.Repository;
using System;

namespace Ragnar.Mock.UnitTesting.InterestV3.Calculator
{
    [TestClass]
    public class InterestCalculatorTest
    {
        private Mock<IBankRepository> bankRepositoryMock;
        private Mock<IPolicyHelper> policyHelperMock;
        private Mock<ITaxHelper> taxHelperMock;
        private Mock<IInterestHelper> interestHelperMock;
        private Mock<IInterestRateHelper> interestRateHelperMock;

        private Mock.InterestV3.Calculator.InterestCalculator interestCalculator;

        [TestInitialize]
        public void TestInitialize()
        {
            bankRepositoryMock = new Mock<IBankRepository>(MockBehavior.Strict);
            policyHelperMock = new Mock<IPolicyHelper>(MockBehavior.Strict);
            taxHelperMock = new Mock<ITaxHelper>(MockBehavior.Strict);
            interestHelperMock = new Mock<IInterestHelper>(MockBehavior.Strict);
            interestRateHelperMock = new Mock<IInterestRateHelper>(MockBehavior.Strict);

            interestCalculator = new Mock.InterestV3.Calculator.InterestCalculator(
                bankRepository: bankRepositoryMock.Object,
                policyHelper: policyHelperMock.Object,
                taxHelper: taxHelperMock.Object,
                interestHelper: interestHelperMock.Object,
                interestRateHelper: interestRateHelperMock.Object);

            MockExtensions.ResetVerifiables();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            MockExtensions.VerifyAll();
        }

        [TestMethod]
        public void ProjectDepositSummary_ApplyInterestRateOnly_ReturnProjectedSummary()
        {
            Bank bank = ScenarioHelper.CreateBank(id: Guid.NewGuid(), name: "BT");

            TaxSystem taxSystem = bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01));

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), amount: 100);

            bankRepositoryMock.Setup_Detail(bank);
            interestRateHelperMock.Setup_ForPeriod(bankInterestRate: bankInterestRate, start: deposit.StartDate, end: deposit.EndDate, periodInterestRate: 0.03M);
            policyHelperMock.Setup_ApplyPolicy(ScenarioHelper.CreatePolicyCalculationContext(taxSystem.TaxPolicies, deposit), 0M);
            taxHelperMock.Setup_Tax(3.0M, 0M, ScenarioHelper.CreateTax(0M, 0M));
            interestHelperMock.Setup_Interest(3.0M, 0M, ScenarioHelper.CreateInterest(3M, 3M));

            DepositProjectionSummary expectedSummary = ScenarioHelper.CreateDepositProjectionSummary(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), initialAmount: 100M);
            expectedSummary.AddTax(asPercentage: 0M, asValue: 0M);
            expectedSummary.AddInterest(asGross: 3M, asNet: 3M);

            DepositProjectionSummary actualSummary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            RagnarAssert.AreEqual(expectedSummary, actualSummary);
        }

        [TestMethod]
        public void ProjectDepositSummary_ApplyInterestRateAndTaxIfEqual_ReturnProjectedSummary()
        {
            Bank bank = ScenarioHelper.CreateBank(id: Guid.NewGuid(), name: "BT");

            bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01))
                .AddTaxPolicy(policyType: PolicyType.TaxPercentage, comparisonAction: ComparisonAction.Equal, comparisonValue: 480000M, taxValue: 0.16M, order: 0);

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), amount: 480000M);

            bankRepositoryMock.Setup_Detail(bank);
            interestRateHelperMock.Setup_ForPeriod(bankInterestRate: bankInterestRate, start: deposit.StartDate, end: deposit.EndDate, periodInterestRate: 0.03M);
            policyHelperMock.Setup_ApplyPolicy(ScenarioHelper.CreatePolicyCalculationContext(bank.TaxSystem[new DateTime(2017, 01, 01)].TaxPolicies, deposit), 0.16M);
            taxHelperMock.Setup_Tax(14400M, 0.16M, ScenarioHelper.CreateTax(0.16M, 2304M));
            interestHelperMock.Setup_Interest(14400M, 0.16M, ScenarioHelper.CreateInterest(14400M, 12096M));

            DepositProjectionSummary expectedSummary = ScenarioHelper.CreateDepositProjectionSummary(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), initialAmount: 480000M);
            expectedSummary.AddTax(asPercentage: 0.16M, asValue: 2304M);
            expectedSummary.AddInterest(asGross: 14400M, asNet: 12096M);

            DepositProjectionSummary actualSummary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            RagnarAssert.AreEqual(expectedSummary, actualSummary);
        }
    }
}