using System;

namespace Ragnar.Integration.InterestV3.Model
{
    public class BankAccount
    {
        public Guid ID { get; set; }

        public Deposit[] Deposits { get; set; }
    }
}