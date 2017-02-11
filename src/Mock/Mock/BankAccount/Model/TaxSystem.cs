using System;

namespace Ragnar.Mock.BankAccount.Model
{
    public class TaxSystem
    {
        public DateTime StartDate { get; set; }

        public Policy[] Policies { get; set; }
    }

    public struct Policy
    {
        public PolicyType Type { get; set; }

        public ComparisonAction Action { get; set; }

        public object ComparisonValue { get; set; }

        public decimal TaxValue { get; set; }
    }

    public enum PolicyType
    {
        Amount = 1,
        Age = 2
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