using Moq;
using System;
using System.Collections.Generic;

namespace Ragnar.Mock.UnitTesting.InterestV3
{
    public static class MockExtensions
    {
        private delegate void Verifiable();

        private static List<Verifiable> verifiables = new List<Verifiable>();

        public static void ResetVerifiables()
        {
            verifiables = new List<Verifiable>();
        }

        public static void VerifyAll()
        {
            verifiables.ForEach(verifiable => verifiable());
        }

        public static void Setup_Detail(this Mock<Mock.InterestV3.Repository.IBankRepository> bankRepositoryMock, Mock.InterestV3.Model.Bank bank, Guid? userId = null, int? atMost = null)
        {
            bankRepositoryMock.Setup(x => x.Detail(bank.Id, userId ?? ScenarioHelper.userId)).Returns(bank);

            verifiables.Add(() => bankRepositoryMock.Verify(x => x.Detail(bank.Id, userId ?? ScenarioHelper.userId), Times.AtMost(atMost ?? 1)));
        }

        public static void Setup_ForPeriod(this Mock<Mock.InterestV3.Calculator.Helpers.IInterestRateHelper> interestRateHelperMock, Mock.InterestV3.Model.BankInterestRate bankInterestRate, DateTime start, DateTime end, decimal periodInterestRate, int? atMost = null)
        {
            interestRateHelperMock.Setup(x => x.ForPeriod(It.Is<Mock.InterestV3.Model.BankInterestRate>(y => RagnarAssert.Match(bankInterestRate, y)), start, end)).Returns(periodInterestRate);

            verifiables.Add(() => interestRateHelperMock.Verify(x => x.ForPeriod(It.Is<Mock.InterestV3.Model.BankInterestRate>(y => RagnarAssert.Match(bankInterestRate, y)), start, end), Times.AtMost(atMost ?? 1)));
        }

        public static void Setup_ApplyPolicy(this Mock<Mock.InterestV3.Calculator.Helpers.IPolicyHelper> policyHelperMock, Mock.InterestV3.Calculator.Helpers.PolicyCalculationContext policyCalculationContext, decimal taxPercentage, int? atMost = null)
        {
            policyHelperMock.Setup(x => x.ApplyPolicy(It.Is<Mock.InterestV3.Calculator.Helpers.PolicyCalculationContext>(y => RagnarAssert.Match(policyCalculationContext, y)))).Returns(taxPercentage);

            verifiables.Add(() => policyHelperMock.Verify(x => x.ApplyPolicy(It.Is<Mock.InterestV3.Calculator.Helpers.PolicyCalculationContext>(y => RagnarAssert.Match(policyCalculationContext, y))), Times.AtMost(atMost ?? 1)));
        }

        public static void Setup_Tax(this Mock<Mock.InterestV3.Calculator.Helpers.ITaxHelper> taxHelperMock, decimal depostInterest, decimal taxPercentage, Mock.InterestV3.Calculator.Contract.Tax tax, int? atMost = null)
        {
            taxHelperMock.Setup(x => x.Tax(depostInterest, taxPercentage)).Returns(tax);

            verifiables.Add(() => taxHelperMock.Verify(x => x.Tax(depostInterest, taxPercentage), Times.AtMost(atMost ?? 1)));
        }

        public static void Setup_Interest(this Mock<Mock.InterestV3.Calculator.Helpers.IInterestHelper> interestHelperMock, decimal depostInterest, decimal taxPercentage, Mock.InterestV3.Calculator.Contract.Interest interest, int? atMost = null)
        {
            interestHelperMock.Setup(x => x.Interest(depostInterest, taxPercentage)).Returns(interest);

            verifiables.Add(() => interestHelperMock.Verify(x => x.Interest(depostInterest, taxPercentage), Times.AtMost(atMost ?? 1)));
        }
    }
}