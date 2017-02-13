using System;

namespace Ragnar.MockDriven.Interest.Model
{
    public class BankAccount
    {
        public Guid ID { get; set; }

        public Deposit[] Deposits { get; set; }
    }
}