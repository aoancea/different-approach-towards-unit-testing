using System;

namespace Ragnar.IntegrationDriven.Interest.Model
{
    public class TaxSystem
    {
        public DateTime StartDate { get; set; }

        public Policy[] Policies { get; set; }
    }

    public struct Policy
    {
        public int Order { get; set; }

        public PolicyType Type { get; set; }

        public ComparisonAction Action { get; set; }

        public object ComparisonValue { get; set; }

        public decimal TaxValue { get; set; }
    }

    public enum PolicyType
    {
        Amount = 1,
        TaxValue,
        //Age = 2 // this can used as an example of a later implementation where each PolicyType is handled by a PolicyTypeHelper resulting in the addition of more Mocks
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