﻿using System;

namespace Ragnar.Mock.InterestV2.Model
{
    // move it on the User
    public class BankAccount
    {
        public Guid ID { get; set; }

        public Deposit[] Deposits { get; set; }
    }
}