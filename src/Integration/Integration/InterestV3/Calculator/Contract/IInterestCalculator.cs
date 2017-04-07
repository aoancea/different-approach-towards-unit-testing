using System;
using System.Collections.Generic;

namespace Ragnar.Integration.InterestV3.Calculator.Contract
{
    public interface IInterestCalculator
    {
        DepositProjectionSummary ProjectDepositSummary(Guid userId, Guid bankId, Guid depositId);
    }

    public class DepositProjectionSummary
    {
        public Guid DepositID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal InitialAmount { get; set; }

        public Tax Tax { get; set; }

        public Interest Interest { get; set; }
    }

    public class Tax
    {
        public decimal AsPercentage { get; set; }

        public decimal AsValue { get; set; }
    }

    public class Interest
    {
        public decimal AsGross { get; set; }

        public decimal AsNet { get; set; }
    }


    public class ProjectionSummary
    {
        public Dictionary<DateTime, DepositProjectionSummary> DepositMonthlyProjectionSummary { get; set; }
    }
}