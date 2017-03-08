using System;

namespace Ragnar.IntegrationDriven.Interest.Calculator.Helpers
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