namespace Ragnar.Mock.InterestV2.Calculator.Helpers
{
    public interface ITaxHelper
    {
        Contract.Tax Tax(decimal depositInterest, decimal taxPercentage);
    }

    public class TaxHelper : ITaxHelper
    {
        public Contract.Tax Tax(decimal depositInterest, decimal taxPercentage)
        {
            Contract.Tax tax = new Contract.Tax();
            tax.AsPercentage = taxPercentage;
            tax.AsValue = depositInterest * taxPercentage;

            return tax;
        }
    }
}