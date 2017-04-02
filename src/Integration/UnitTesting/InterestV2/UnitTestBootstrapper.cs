using SimpleInjector;

namespace Ragnar.Integration.UnitTesting.InterestV2
{
    public static class UnitTestBootstrapper
    {
        public static void Register(Container container)
        {
            container.Register<Integration.InterestV2.Calculator.Helpers.IComparisonHelper, Integration.InterestV2.Calculator.Helpers.ComparisonHelper>();
            container.Register<Integration.InterestV2.Calculator.Helpers.IInterestHelper, Integration.InterestV2.Calculator.Helpers.InterestHelper>();
            container.Register<Integration.InterestV2.Calculator.Helpers.IPolicyHelper, Integration.InterestV2.Calculator.Helpers.PolicyHelper>();
            container.Register<Integration.InterestV2.Calculator.Helpers.ITaxHelper, Integration.InterestV2.Calculator.Helpers.TaxHelper>();

            container.Verify();
        }
    }
}
