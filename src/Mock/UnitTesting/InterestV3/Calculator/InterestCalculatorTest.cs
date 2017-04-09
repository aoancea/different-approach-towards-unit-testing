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
        }

        [TestCleanup]
        public void TestCleanup()
        {
            bankRepositoryMock.VerifyAll();
            policyHelperMock.VerifyAll();
            taxHelperMock.VerifyAll();
            interestHelperMock.VerifyAll();
            interestRateHelperMock.VerifyAll();
        }

        [TestMethod]
        public void ProjectDepositSummary_ApplyInterestRateOnly_ReturnProjectedSummary()
        {
            Bank bank = ScenarioHelper.CreateBank(id: Guid.NewGuid(), name: "BT");

            TaxSystem taxSystem = bank.AddTaxSystem(effectiveDate: new DateTime(2017, 01, 01));

            BankInterestRate bankInterestRate = bank.AddInterestRate(startDate: new DateTime(2017, 01, 01), value: 0.03M);

            BankAccount bankAccount = bank.AddBankAccount(id: Guid.NewGuid());

            Deposit deposit = bankAccount.AddDeposit(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), amount: 100);

            bankRepositoryMock.Setup(x => x.Detail(bank.Id, ScenarioHelper.userId)).Returns(bank);
            // How did we get to this return value? Well we ran the tests from the Integration side or did the calculation our selves. What happens if the bussiness logic is more complex and we don't have any tests that integrate everything? We pretty much end up writing an invalid scenario
            interestRateHelperMock.Setup(x => x.ForPeriod(bankInterestRate, deposit.StartDate, deposit.EndDate)).Returns(0.03M);

            policyHelperMock.Setup(x => x.ApplyPolicy(It.Is<PolicyCalculationContext>(y => RagnarAssert.Match(ScenarioHelper.CreatePolicyCalculationContext(taxSystem.TaxPolicies, deposit), y)))).Returns(0M);

            taxHelperMock.Setup(x => x.Tax(3.0M, 0M)).Returns(new Tax() { AsPercentage = 0M, AsValue = 0M });
            interestHelperMock.Setup(x => x.Interest(3.0M, 0M)).Returns(new Mock.InterestV3.Calculator.Contract.Interest() { AsGross = 3M, AsNet = 3M });

            DepositProjectionSummary expectedSummary = ScenarioHelper.CreateDepositProjectionSummary(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), initialAmount: 100M);
            expectedSummary.AddInterest(asGross: 3M, asNet: 3M);
            expectedSummary.AddTax(asPercentage: 0M, asValue: 0M);

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

            bankRepositoryMock.Setup(x => x.Detail(bank.Id, ScenarioHelper.userId)).Returns(bank);
            // How did we get to this return value? Well we ran the tests from the Integration side or did the calculation our selves. What happens if the bussiness logic is more complex and we don't have any tests that integrate everything? We pretty much end up writing an invalid scenario
            interestRateHelperMock.Setup(x => x.ForPeriod(bankInterestRate, deposit.StartDate, deposit.EndDate)).Returns(0.03M);

            policyHelperMock.Setup(x => x.ApplyPolicy(It.Is<PolicyCalculationContext>(y => RagnarAssert.Match(ScenarioHelper.CreatePolicyCalculationContext(bank.TaxSystem[new DateTime(2017, 01, 01)].TaxPolicies, deposit), y)))).Returns(0.16M);

            taxHelperMock.Setup(x => x.Tax(14400M, 0.16M)).Returns(new Tax() { AsPercentage = 0.16M, AsValue = 2304M });
            interestHelperMock.Setup(x => x.Interest(14400M, 0.16M)).Returns(new Mock.InterestV3.Calculator.Contract.Interest() { AsGross = 14400M, AsNet = 12096M });

            DepositProjectionSummary expectedSummary = ScenarioHelper.CreateDepositProjectionSummary(startDate: new DateTime(2017, 01, 01), endDate: new DateTime(2017, 12, 31), initialAmount: 480000M);
            expectedSummary.AddInterest(asGross: 14400M, asNet: 12096M);
            expectedSummary.AddTax(asPercentage: 0.16M, asValue: 2304M);

            DepositProjectionSummary actualSummary = interestCalculator.ProjectDepositSummary(ScenarioHelper.userId, bank.Id, deposit.ID);

            RagnarAssert.AreEqual(expectedSummary, actualSummary);
        }
    }
}