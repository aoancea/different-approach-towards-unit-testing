using System;

namespace Ragnar.IntegrationDriven.InterestV2.Model
{
    public class BankAccount
    {
        public Guid ID { get; set; }

        public Deposit[] Deposits { get; set; }
    }
}