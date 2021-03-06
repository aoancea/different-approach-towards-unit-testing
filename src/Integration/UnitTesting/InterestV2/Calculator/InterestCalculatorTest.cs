﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ragnar.Integration.InterestV2.Calculator.Contract;
using Ragnar.Integration.InterestV2.Calculator.Helpers;
using Ragnar.Integration.InterestV2.Model;
using Ragnar.Integration.InterestV2.Repository;
using SimpleInjector;
using System;

namespace Ragnar.Integration.UnitTesting.InterestV2.Calculator
{
    [TestClass]
    public class InterestCalculatorTest
    {
        private Mock<IBankRepository> bankRepositoryMock;

        private Container container;

        private Integration.InterestV2.Calculator.InterestCalculator interestCalculator;

        [TestInitialize]
        public void TestInitialize()
        {
            bankRepositoryMock = new Mock<IBankRepository>(MockBehavior.Strict);

            container = new Container();

            UnitTestBootstrapper.Register(container);

            interestCalculator = new Integration.InterestV2.Calculator.InterestCalculator(
                bankRepository: bankRepositoryMock.Object,
                policyHelper: container.GetInstance<IPolicyHelper>(),
                taxHelper: container.GetInstance<ITaxHelper>(),
                interestHelper: container.GetInstance<IInterestHelper>(),
                interestRateHelper: container.GetInstance<InterestRateHelper>());
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
            Assert.AreEqual(3M, summary.Interest.AsGross);
            Assert.AreEqual(3M, summary.Interest.AsNet);
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
            Assert.AreEqual(14400M, summary.Interest.AsGross);
            Assert.AreEqual(12096M, summary.Interest.AsNet);
            Assert.AreEqual(0.16M, summary.Tax.AsPercentage);
            Assert.AreEqual(2304M, summary.Tax.AsValue);
        }

        private void SetupMocks(Bank bank)
        {
            bankRepositoryMock.Setup(x => x.Detail(bank.Id, ScenarioHelper.userId)).Returns(bank);
        }
    }
}
