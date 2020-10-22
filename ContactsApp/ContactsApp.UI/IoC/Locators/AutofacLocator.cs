using Autofac;

using ContactsApp.UI.IoC.Infra;
using ContactsApp.UI.IoC.Extensions;
using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.IoC.Locators
{
    public class AutofacLocator
    {
        private readonly IModule _module;
        private readonly IContainer _container;
        private readonly ContainerBuilder _builder;

        public AutofacLocator(IModule module)
        {
            _module = module.VerifyReferenceAndSet(nameof(module));

            _builder = new ContainerBuilder();
            
            this.AddBindings();

            _container = _builder.Build();
        }

        private void AddBindings()
        {
            var components = _module.GetManagedComponents();

            foreach (var component in components)
            {
                if (component.WithCtorArgs)
                {
                    _builder.RegisterWithCtorArgs(component);
                }
                else
                {
                    _builder.RegisterArgsEmpty(component);
                }
            }
        }
        public TInstance GetInstance<TInstance>()
        {
            return _container.Resolve<TInstance>();
        }
    }
}