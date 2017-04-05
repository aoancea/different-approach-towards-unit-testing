using System;

namespace Ragnar.Mock.InterestV2.Calculator.Helpers
{
    public interface IInterestRateHelper
    {
        decimal ForPeriod(Model.BankInterestRate interestRate, DateTime start, DateTime end);
    }

    public class InterestRateHelper : IInterestRateHelper
    {
        private readonly IRangeHelper rangeHelper;

        public InterestRateHelper(IRangeHelper rangeHelper)
        {
            this.rangeHelper = rangeHelper;
        }

        public decimal ForPeriod(Model.BankInterestRate interestRate, DateTime start, DateTime end)
        {
            int interestRateDaysActive = rangeHelper.DaysBetween(start, end);

            return interestRateDaysActive / 365 * interestRate.Value; // and yes, we don't care for bisect year in this example
        }
    }
}
