using System.Collections.Generic;
using System.Reflection;

namespace Ragnar.Integration.UnitTesting.Dispatcher
{
    public static class Bootstrapper
    {
        public static void Initialize(SimpleInjector.Container container)
        {
            List<Assembly> assemblies = new List<Assembly>(new[] { Assembly.GetCallingAssembly() });

            container.Register<Integration.Dispatcher.Authorization.IAuthorizer, AuthorizerMock>();

            container.Register(typeof(Integration.Dispatcher.Authorization.CommandAuthorizerBase<>), assemblies);
            container.Register(typeof(Integration.Dispatcher.Validation.CommandValidatorBase<>), assemblies);
            container.Register(typeof(Integration.Dispatcher.ICommandHandler<>), assemblies);

            container.Register<Integration.Dispatcher.CommandDispatcher>();

            container.Verify();
        }
    }

    public class AuthorizerMock : Integration.Dispatcher.Authorization.IAuthorizer
    {
        public bool CheckAccess()
        {
            return true;
        }
    }
}
