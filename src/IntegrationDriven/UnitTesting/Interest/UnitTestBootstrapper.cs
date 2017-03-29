using SimpleInjector;

namespace Ragnar.IntegrationDriven.UnitTesting.Interest
{
    public static class UnitTestBootstrapper
    {
        public static void Register(Container container)
        {
            container.Register<IntegrationDriven.Interest.Calculator.Helpers.IComparisonHelper, IntegrationDriven.Interest.Calculator.Helpers.ComparisonHelper>();
            container.Register<IntegrationDriven.Interest.Calculator.Helpers.IInterestHelper, IntegrationDriven.Interest.Calculator.Helpers.InterestHelper>();
            container.Register<IntegrationDriven.Interest.Calculator.Helpers.IPolicyHelper, IntegrationDriven.Interest.Calculator.Helpers.PolicyHelper>();
            container.Register<IntegrationDriven.Interest.Calculator.Helpers.ITaxHelper, IntegrationDriven.Interest.Calculator.Helpers.TaxHelper>();

            container.Verify();
        }
    }
}
