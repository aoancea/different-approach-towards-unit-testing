using System;

namespace Ragnar.MockDriven.Interest.Calculator.Helpers
{
    public interface IPolicyHelper
    {
        decimal ApplyPolicy(Model.Policy policy);
    }

    public class PolicyHelper : IPolicyHelper
    {
        public decimal ApplyPolicy(Model.Policy policy)
        {
            throw new NotImplementedException();
        }
    }
}