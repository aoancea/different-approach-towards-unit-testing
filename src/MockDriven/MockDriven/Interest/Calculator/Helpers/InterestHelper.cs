namespace Ragnar.MockDriven.Interest.Calculator.Helpers
{
    public interface IInterestHelper
    {
        Contract.Interest Interest(decimal amount, decimal taxPercentage);
    }

    public class InterestHelper : IInterestHelper
    {
        public Contract.Interest Interest(decimal amount, decimal taxPercentage)
        {
            Contract.Interest interest = new Contract.Interest();
            interest.AsGross = amount;
            interest.AsNet = amount - (amount * taxPercentage);

            return interest;
        }
    }
}