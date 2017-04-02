using System;

namespace Ragnar.Integration.Interest.Model
{
    public struct BankInterestRate
    {
        public DateTime StartDate { get; set; }

        public decimal Value { get; set; }
    }
}