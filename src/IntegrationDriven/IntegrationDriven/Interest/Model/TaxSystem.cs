using System;

namespace Ragnar.IntegrationDriven.Interest.Model
{
    public class TaxSystem
    {
        public DateTime StartDate { get; set; }

        public TaxPolicy[] TaxPolicies { get; set; }
    }

    public class TaxPolicy
    {
        public PolicyType PolicyType { get; set; }

        public ComparisonAction ComparisonAction { get; set; }

        public object ComparisonValue { get; set; }

        public decimal TaxValue { get; set; }

        public int Order { get; set; }
    }

    public enum PolicyType
    {
        TaxPercentage = 1, // this is the default or global tax percentage
    }

    public enum ComparisonAction
    {
        LessThan,
        LessOrEqualThan,
        Equal,
        GreaterThan,
        GreaterOrEqualThan
    }
}