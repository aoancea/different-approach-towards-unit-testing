using System;

namespace Ragnar.MockDriven.Interest.Calculator.Helpers
{
    public interface ITaxHelper
    {
        Contract.Tax ConvertoToTax();
    }

    public class TaxHelper : ITaxHelper
    {
        public Contract.Tax ConvertoToTax()
        {
            throw new NotImplementedException();
        }
    }
}