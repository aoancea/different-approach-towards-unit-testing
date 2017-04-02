using System;

namespace Ragnar.Integration.Interest.Model
{
    public class BankAccount
    {
        public Guid ID { get; set; }

        public Deposit[] Deposits { get; set; }
    }
}