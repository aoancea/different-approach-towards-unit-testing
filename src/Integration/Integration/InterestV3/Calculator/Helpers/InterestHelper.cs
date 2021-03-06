﻿namespace Ragnar.Integration.InterestV3.Calculator.Helpers
{
    public interface IInterestHelper
    {
        Contract.Interest Interest(decimal amount, decimal taxPercentange);
    }

    public class InterestHelper : IInterestHelper
    {
        public Contract.Interest Interest(decimal amount, decimal taxPercentange)
        {
            Contract.Interest interest = new Contract.Interest();
            interest.AsGross = amount;
            interest.AsNet = amount - (amount * taxPercentange);

            return interest;
        }
    }
}