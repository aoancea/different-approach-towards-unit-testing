using System;

namespace Ragnar.IntegrationDriven.Interest.Model
{
    public class BankAccount
    {
        public Guid ID { get; set; }

        public Deposit[] Deposits { get; set; }
    }
}