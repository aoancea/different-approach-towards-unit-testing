using System;
using System.Linq;

namespace Ragnar.Integration.InterestV3.Calculator
{
    public class InterestCalculator : Contract.IInterestCalculator
    {
        private readonly Repository.IBankRepository bankRepository;
        private readonly Helpers.IPolicyHelper policyHelper;
        private readonly Helpers.ITaxHelper taxHelper;
        private readonly Helpers.IInterestHelper interestHelper;
        private readonly Helpers.IInterestRateHelper interestRateHelper;

        public InterestCalculator(
            Repository.IBankRepository bankRepository,
            Helpers.IPolicyHelper policyHelper,
            Helpers.ITaxHelper taxHelper,
            Helpers.IInterestHelper interestHelper,
            Helpers.IInterestRateHelper interestRateHelper)
        {
            this.bankRepository = bankRepository;
            this.policyHelper = policyHelper;
            this.taxHelper = taxHelper;
            this.interestHelper = interestHelper;
            this.interestRateHelper = interestRateHelper;
        }

        public Contract.DepositProjectionSummary ProjectDepositSummary(Guid userId, Guid bankId, Guid depositId)
        {
            Model.Bank bank = bankRepository.Detail(bankId, userId);

            Model.Deposit deposit = bank.BankAccount.Deposits.First(x => x.ID == depositId);

            Model.BankInterestRate interestRate = bank.Rates.First(x => x.StartDate >= deposit.StartDate && x.StartDate <= deposit.EndDate);

            Model.TaxSystem taxSystem = bank.TaxSystem.First(x => x.Key >= deposit.StartDate && x.Key <= deposit.EndDate).Value;

            decimal depositInterest = deposit.Amount * interestRateHelper.ForPeriod(interestRate, deposit.StartDate, deposit.EndDate);

            decimal taxPercentage = policyHelper.ApplyPolicy(CreatePolicyCalcContext(taxSystem, deposit));

            Contract.Tax tax = taxHelper.Tax(depositInterest, taxPercentage);
            Contract.Interest interest = interestHelper.Interest(depositInterest, taxPercentage);

            return new Contract.DepositProjectionSummary()
            {
                DepositID = depositId,
                StartDate = deposit.StartDate,
                EndDate = deposit.EndDate,
                InitialAmount = deposit.Amount,
                Interest = interest,
                Tax = tax
            };
        }

        private Helpers.PolicyCalculationContext CreatePolicyCalcContext(Model.TaxSystem taxSystem, Model.Deposit deposit)
        {
            return new Helpers.PolicyCalculationContext() { TaxPolicies = taxSystem.TaxPolicies, Deposit = deposit };
        }
    }
}