using System;
using System.Linq;

namespace Ragnar.IntegrationDriven.Interest.Calculator
{
    public class InterestCalculator : Contract.IInterestCalculator
    {
        private readonly Repository.IBankRepository bankRepository;
        private readonly Helpers.IPolicyHelper policyHelper;
        private readonly Helpers.ITaxHelper taxHelper;
        private readonly Helpers.IInterestHelper interestHelper;
        private readonly Helpers.IComparisonHelper comparisonHelper;

        public InterestCalculator(
            Repository.IBankRepository bankRepository,
            Helpers.IPolicyHelper policyHelper,
            Helpers.ITaxHelper taxHelper,
            Helpers.IInterestHelper interestHelper,
            Helpers.IComparisonHelper comparisonHelper)
        {
            this.bankRepository = bankRepository;
            this.policyHelper = policyHelper;
            this.taxHelper = taxHelper;
            this.interestHelper = interestHelper;
            this.comparisonHelper = comparisonHelper;
        }

        public Contract.DepositProjectionSummary ProjectDepositSummary(Guid userId, Guid bankId, Guid depositId)
        {
            Model.Bank bank = bankRepository.Detail(bankId, userId);

            Model.Deposit deposit = bank.BankAccount.Deposits.First(x => x.ID == depositId);

            Model.BankInterestRate interestRate = bank.Rates.First(x => x.StartDate >= deposit.StartDate && x.StartDate <= deposit.EndDate);

            Model.TaxSystem taxSystem = bank.TaxSystem.First(x => x.Key >= deposit.StartDate && x.Key <= deposit.EndDate).Value;

            // move them first in their own Helpers
            // then remove the first helper and move the code in the 2nd one
            int depositDaysActive = DepositDaysActive(deposit); // loose this and build it inside ActualInterestRate

            decimal actualInterestRate = ActualInterestRate(interestRate, depositDaysActive);

            decimal depositInterest = deposit.Amount * actualInterestRate;

            decimal taxPercentage = 0;
            foreach (Model.TaxPolicy taxPolicy in taxSystem.TaxPolicies)
            {
                switch (taxPolicy.Type)
                {
                    case Model.PolicyType.TaxPercentage:
                        {
                            if (comparisonHelper.Compare(taxPolicy.Action, deposit.Amount, (decimal)taxPolicy.ComparisonValue))
                            {
                                taxPercentage += taxPolicy.TaxValue;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

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

        private int DepositDaysActive(Model.Deposit deposit)
        {
            return (int)(deposit.EndDate - deposit.StartDate).TotalDays;
        }

        private decimal ActualInterestRate(Model.BankInterestRate interestRate, int depositDaysActive)
        {
            decimal interestPerDay = interestRate.Value / 365;

            return interestPerDay * depositDaysActive;
        }
    }
}