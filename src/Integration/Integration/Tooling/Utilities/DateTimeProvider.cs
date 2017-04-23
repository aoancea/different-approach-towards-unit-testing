using System;

namespace Ragnar.Integration.Tooling.Utilities
{
    public interface IDateTimeProvider
    {
        DateTime DateTime();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime DateTime()
        {
            return System.DateTime.Now;
        }
    }
}