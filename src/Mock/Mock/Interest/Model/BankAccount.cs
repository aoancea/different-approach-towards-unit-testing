﻿using System;

namespace Ragnar.MockDriven.Interest.Model
{
    public class BankAccount
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        //public TaxSystem TaxSystem { get; set; } // current TaxSystem applies globally based on StartDate(which is the date it becomes effective). We might need to apply it on individual bank accounts so that people can customize they Interest. We might also want to support certain promotion to certain Users. Another approach would be to have a another TaxSystem which overrides only certain policies from the ones that apply globally
    }
}