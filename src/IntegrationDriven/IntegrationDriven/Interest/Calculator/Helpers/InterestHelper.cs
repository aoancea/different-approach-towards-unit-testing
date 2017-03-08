namespace Ragnar.IntegrationDriven.Interest.Calculator.Helpers
{
    public interface IInterestHelper
    {
        Contract.Interest Interest(decimal amount, Contract.Tax tax);
    }

    public class InterestHelper : IInterestHelper
    {
        public Contract.Interest Interest(decimal amount, Contract.Tax tax)
        {
            Contract.Interest interest = new Contract.Interest();
            interest.AsGross = amount;
            interest.AsNet = amount - tax.AsValue;

            return interest;
        }
    }
}