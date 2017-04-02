namespace Ragnar.Mock.InterestV2.Calculator.Helpers
{
    public interface IInterestHelper
    {
        Contract.Interest Interest(decimal depositInterest, decimal taxPercentage);
    }

    public class InterestHelper : IInterestHelper
    {
        public Contract.Interest Interest(decimal depositInterest, decimal taxPercentage)
        {
            Contract.Interest interest = new Contract.Interest();
            interest.AsGross = depositInterest;
            interest.AsNet = depositInterest - (depositInterest * taxPercentage);

            return interest;
        }
    }
}