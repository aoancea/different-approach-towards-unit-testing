﻿using System;

namespace Ragnar.IntegrationDriven.InterestV2.Calculator.Helpers
{
    public interface IPolicyHelper
    {
        decimal ApplyPolicy(Model.TaxPolicy policy);
    }

    public class PolicyHelper : IPolicyHelper
    {
        public decimal ApplyPolicy(Model.TaxPolicy policy)
        {
            throw new NotImplementedException();
        }
    }
}