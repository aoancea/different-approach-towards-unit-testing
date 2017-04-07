using System;

namespace Ragnar.Integration.InterestV3.Calculator.Helpers
{
    public interface IPolicyHelper
    {
        decimal ApplyPolicy(PolicyCalculationContext policyCalcContext);
    }

    public class PolicyHelper : IPolicyHelper
    {
        private readonly IComparisonHelper comparisonHelper;

        public PolicyHelper(IComparisonHelper comparisonHelper)
        {
            this.comparisonHelper = comparisonHelper;
        }

        public decimal ApplyPolicy(PolicyCalculationContext policyCalcContext)
        {
            decimal taxPercentage = decimal.Zero;
            foreach (Model.TaxPolicy taxPolicy in policyCalcContext.TaxPolicies)
            {
                switch (taxPolicy.PolicyType)
                {
                    case Model.PolicyType.TaxPercentage:
                        {
                            if (comparisonHelper.Compare(taxPolicy.ComparisonAction, policyCalcContext.Deposit.Amount, (decimal)taxPolicy.ComparisonValue))
                            {
                                taxPercentage += taxPolicy.TaxValue;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            return taxPercentage;
        }
    }

    public class PolicyCalculationContext
    {
        public Model.TaxPolicy[] TaxPolicies { get; set; }

        public Model.Deposit Deposit { get; set; }
    }
}