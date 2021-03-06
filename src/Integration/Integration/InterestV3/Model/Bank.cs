﻿using System;
using System.Collections.Generic;

namespace Ragnar.Integration.InterestV3.Model
{
    public class Bank
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public BankInterestRate[] Rates { get; set; }

        public Dictionary<DateTime, TaxSystem> TaxSystem { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}