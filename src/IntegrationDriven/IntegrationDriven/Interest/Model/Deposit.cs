using System;

namespace Ragnar.IntegrationDriven.Interest.Model
{
    public class Deposit
    {
        public Guid ID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Amount { get; set; }

        //public TaxSystem TaxSystem { get; set; } // current TaxSystem applies globally based on StartDate(which is the date it becomes effective). We might need to apply it on individual bank accounts so that people can customize they Interest. We might also want to support certain promotion to certain Users. Another approach would be to have a another TaxSystem which overrides only certain policies from the ones that apply globally
    }
}
