using System;

namespace Ragnar.Mock.BankAccount.Model
{
    public struct BankInterestRate
    {
        public DateTime StartDate { get; set; }

        public decimal Value { get; set; }
    }
}