namespace Ragnar.Mock.BankAccount.Model
{
    public class Bank
    {
        public string Name { get; set; }

        public BankInterestRate[] Rates { get; set; }
    }
}