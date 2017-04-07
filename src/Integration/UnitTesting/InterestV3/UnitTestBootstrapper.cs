using SimpleInjector;

namespace Ragnar.Integration.UnitTesting.InterestV3
{
    public static class UnitTestBootstrapper
    {
        public static void Register(Container container)
        {
            container.Register<Integration.InterestV3.Calculator.Helpers.IComparisonHelper, Integration.InterestV3.Calculator.Helpers.ComparisonHelper>();
            container.Register<Integration.InterestV3.Calculator.Helpers.IInterestHelper, Integration.InterestV3.Calculator.Helpers.InterestHelper>();
            container.Register<Integration.InterestV3.Calculator.Helpers.IPolicyHelper, Integration.InterestV3.Calculator.Helpers.PolicyHelper>();
            container.Register<Integration.InterestV3.Calculator.Helpers.ITaxHelper, Integration.InterestV3.Calculator.Helpers.TaxHelper>();
            container.Register<Integration.InterestV3.Calculator.Helpers.IRangeHelper, Integration.InterestV3.Calculator.Helpers.RangeHelper>();
            container.Register<Integration.InterestV3.Calculator.Helpers.IInterestRateHelper, Integration.InterestV3.Calculator.Helpers.InterestRateHelper>();

            container.Verify();
        }
    }
}
