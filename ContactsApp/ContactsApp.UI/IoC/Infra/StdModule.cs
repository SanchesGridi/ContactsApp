using System.Collections.Generic;

using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.IoC.Infra
{
    public class StdModule : IModule
    {
        private readonly IEnumerable<IManagedComponent> _componentCollection;

        public StdModule(IEnumerable<IManagedComponent> components)
        {
            _componentCollection = components.VerifyReferenceAndSet(nameof(components));
        }

        public IEnumerable<IManagedComponent> GetManagedComponents()
        {
            return _componentCollection;
        }
    }
}