using System;
using System.Collections.Generic;

namespace Ragnar.MockDriven.Interest.Model
{
    public class Bank
    {
        public string Name { get; set; }

        public BankInterestRate[] Rates { get; set; }

        public Dictionary<DateTime, TaxSystem> TaxSystem { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}