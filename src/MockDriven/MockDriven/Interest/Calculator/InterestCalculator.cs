using System;
using System.Linq;

namespace Ragnar.MockDriven.Interest.Calculator
{
    public class InterestCalculator : Contract.IInterestCalculator
    {
        private readonly Repository.IBankRepository bankRepository;
        private readonly Helpers.IPolicyHelper policyHelper;
        private readonly Helpers.ITaxHelper taxHelper;
        private readonly Helpers.IInterestHelper interestHelper;

        public InterestCalculator(
            Repository.IBankRepository bankRepository,
            Helpers.IPolicyHelper policyHelper,
            Helpers.ITaxHelper taxHelper,
            Helpers.IInterestHelper interestHelper)
        {
            this.bankRepository = bankRepository;
            this.policyHelper = policyHelper;
            this.taxHelper = taxHelper;
            this.interestHelper = interestHelper;
        }

        public Contract.DepositProjectionSummary ProjectDepositSummary(Guid userId, Guid bankId, Guid depositId)
        {
            Model.Bank bank = bankRepository.Detail(bankId, userId);

            Model.Deposit deposit = bank.BankAccount.Deposits.First(x => x.ID == depositId);

            Model.BankInterestRate interestRate = bank.Rates.First(x => x.StartDate >= deposit.StartDate && x.StartDate <= deposit.EndDate);

            Model.TaxSystem taxSystem = bank.TaxSystem.First(x => x.Key >= deposit.StartDate && x.Key <= deposit.EndDate).Value;

            int depositDaysActive = DepositDaysActive(deposit);

            decimal actualInterestRate = ActualInterestRate(interestRate, depositDaysActive);

            decimal depositInterest = deposit.Amount * actualInterestRate;

            decimal taxValue = 0;
            foreach (Model.Policy policy in taxSystem.Policies)
            {
                //policyHelper.ApplyPolicy(policy);
                switch (policy.Type)
                {
                    case Model.PolicyType.Amount:
                        {
                            if (Compare(policy.Action, deposit.Amount, (decimal)policy.ComparisonValue))
                            {
                                taxValue += policy.TaxValue;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            Contract.Tax tax = taxHelper.Tax(depositInterest, taxValue);
            Contract.Interest interest = interestHelper.Interest(depositInterest, tax);

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


        private bool Compare<T>(Model.ComparisonAction comparisonAction, T value1, T value2) where T : IComparable
        {
            switch (comparisonAction)
            {
                case Model.ComparisonAction.LessThan:
                    return CompareLessThan(value1, value2);

                case Model.ComparisonAction.LessOrEqualThan:
                    return CompareLessThan(value1, value2) || CompareEqual(value1, value2);

                case Model.ComparisonAction.Equal:
                    return CompareEqual(value1, value2);

                case Model.ComparisonAction.GreaterOrEqualThan:
                    return CompareGreaterThan(value1, value2) || CompareEqual(value1, value2);

                case Model.ComparisonAction.GreaterThan:
                    return CompareGreaterThan(value1, value2);

                default:
                    return false;
            }
        }

        private bool CompareLessThan<T>(T value1, T value2) where T : IComparable
        {
            if (value1 == null && value2 == null)
                return true;

            if (value1 == null || value2 == null)
                return false;

            Type value1Type = value1.GetType();
            Type value2Type = value2.GetType();

            if (value1Type != value2Type)
                return false;

            return value1.CompareTo(value2) < 0;
        }

        private bool CompareEqual<T>(T value1, T value2) where T : IComparable
        {
            if (value1 == null && value2 == null)
                return true;

            if (value1 == null || value2 == null)
                return false;

            Type value1Type = value1.GetType();
            Type value2Type = value2.GetType();

            if (value1Type != value2Type)
                return false;

            return value1.CompareTo(value2) == 0;
        }

        private bool CompareGreaterThan<T>(T value1, T value2) where T : IComparable
        {
            if (value1 == null && value2 == null)
                return true;

            if (value1 == null || value2 == null)
                return false;

            Type value1Type = value1.GetType();
            Type value2Type = value2.GetType();

            if (value1Type != value2Type)
                return false;

            return value1.CompareTo(value2) > 0;
        }
    }
}