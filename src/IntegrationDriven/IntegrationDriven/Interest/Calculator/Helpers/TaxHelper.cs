namespace Ragnar.IntegrationDriven.Interest.Calculator.Helpers
{
    public interface ITaxHelper
    {
        Contract.Tax Tax(decimal depositInterest, decimal taxValue);
    }

    public class TaxHelper : ITaxHelper
    {
        public Contract.Tax Tax(decimal depositInterest, decimal taxValue)
        {
            Contract.Tax tax = new Contract.Tax();
            tax.AsPercentage = taxValue;
            tax.AsValue = depositInterest * taxValue;

            return tax;
        }
    }
}