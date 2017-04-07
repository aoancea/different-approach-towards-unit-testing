using System;

namespace Ragnar.Integration.InterestV3.Model
{
    public struct BankInterestRate
    {
        public DateTime StartDate { get; set; }

        public decimal Value { get; set; }
    }
}