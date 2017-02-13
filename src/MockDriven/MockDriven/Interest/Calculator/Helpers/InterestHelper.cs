namespace Ragnar.MockDriven.Interest.Calculator.Helpers
{
    public interface IInterestHelper
    {
        Contract.Interest BuildInterest(decimal amount, Contract.Tax tax);
    }

    public class InterestHelper : IInterestHelper
    {
        public Contract.Interest BuildInterest(decimal amount, Contract.Tax tax)
        {
            return null;
        }
    }
}