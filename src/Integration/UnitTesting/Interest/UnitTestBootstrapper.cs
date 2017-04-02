using SimpleInjector;

namespace Ragnar.Integration.UnitTesting.Interest
{
    public static class UnitTestBootstrapper
    {
        public static void Register(Container container)
        {
            container.Register<Integration.Interest.Calculator.Helpers.IComparisonHelper, Integration.Interest.Calculator.Helpers.ComparisonHelper>();
            container.Register<Integration.Interest.Calculator.Helpers.IInterestHelper, Integration.Interest.Calculator.Helpers.InterestHelper>();
            container.Register<Integration.Interest.Calculator.Helpers.IPolicyHelper, Integration.Interest.Calculator.Helpers.PolicyHelper>();
            container.Register<Integration.Interest.Calculator.Helpers.ITaxHelper, Integration.Interest.Calculator.Helpers.TaxHelper>();

            container.Verify();
        }
    }
}
