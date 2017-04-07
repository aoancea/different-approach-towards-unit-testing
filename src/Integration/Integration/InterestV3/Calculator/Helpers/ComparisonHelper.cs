using System;

namespace Ragnar.Integration.InterestV3.Calculator.Helpers
{
    public interface IComparisonHelper
    {
        bool Compare<T>(Model.ComparisonAction comparisonAction, T value1, T value2) where T : IComparable;
    }

    public class ComparisonHelper : IComparisonHelper
    {
        public bool Compare<T>(Model.ComparisonAction comparisonAction, T value1, T value2) where T : IComparable
        {
            switch (comparisonAction)
            {
                case Model.ComparisonAction.LessThan:
                    return CompareLessThan(value1, value2);

                case Model.ComparisonAction.LessOrEqualThan:
                    return CompareLessThan(value1, value2) || CompareEqual(value1, value2);

                case Model.ComparisonAction.Equal:
                    return CompareEqual(value1, value2);

                case Model.ComparisonAction.GreaterOrEqualThan:
                    return CompareGreaterThan(value1, value2) || CompareEqual(value1, value2);

                case Model.ComparisonAction.GreaterThan:
                    return CompareGreaterThan(value1, value2);

                default:
                    return false;
            }
        }

        private bool CompareLessThan<T>(T value1, T value2) where T : IComparable
        {
            if (value1 == null && value2 == null)
                return true;

            if (value1 == null || value2 == null)
                return false;

            Type value1Type = value1.GetType();
            Type value2Type = value2.GetType();

            if (value1Type != value2Type)
                return false;

            return value1.CompareTo(value2) < 0;
        }

        private bool CompareEqual<T>(T value1, T value2) where T : IComparable
        {
            if (value1 == null && value2 == null)
                return true;

            if (value1 == null || value2 == null)
                return false;

            Type value1Type = value1.GetType();
            Type value2Type = value2.GetType();

            if (value1Type != value2Type)
                return false;

            return value1.CompareTo(value2) == 0;
        }

        private bool CompareGreaterThan<T>(T value1, T value2) where T : IComparable
        {
            if (value1 == null && value2 == null)
                return true;

            if (value1 == null || value2 == null)
                return false;

            Type value1Type = value1.GetType();
            Type value2Type = value2.GetType();

            if (value1Type != value2Type)
                return false;

            return value1.CompareTo(value2) > 0;
        }
    }
}