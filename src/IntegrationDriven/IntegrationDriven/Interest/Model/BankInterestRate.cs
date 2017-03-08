using System;

namespace Ragnar.IntegrationDriven.Interest.Model
{
    public struct BankInterestRate
    {
        public DateTime StartDate { get; set; }

        public decimal Value { get; set; }
    }
}