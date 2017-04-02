using System;

namespace Ragnar.IntegrationDriven.InterestV2.Model
{
    public struct BankInterestRate
    {
        public DateTime StartDate { get; set; }

        public decimal Value { get; set; }
    }
}