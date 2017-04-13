using System;

namespace Ragnar.Integration.Dispatcher
{
    public interface IIocContainer
    {
        object GetInstance(Type type);
    }

    public class IocContainer : IIocContainer
    {
        private readonly SimpleInjector.Container container;

        public IocContainer(SimpleInjector.Container container)
        {
            this.container = container;
        }

        public object GetInstance(Type type)
        {
            return container.GetInstance(type);
        }
    }
}