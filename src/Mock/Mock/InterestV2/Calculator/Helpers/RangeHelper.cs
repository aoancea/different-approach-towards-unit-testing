using System;

namespace Ragnar.Mock.InterestV2.Calculator.Helpers
{
    public interface IRangeHelper
    {
        int DaysBetween(DateTime start, DateTime end);
    }

    public class RangeHelper : IRangeHelper
    {
        public int DaysBetween(DateTime start, DateTime end)
        {
            return (int)(end - start).TotalDays + 1;
        }
    }
}
