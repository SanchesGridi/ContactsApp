using System.Collections.Generic;

using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.IoC.Infra
{
    public class MacroModule : IModule
    {
        private readonly IEnumerable<IModule> _modules;

        public MacroModule(IEnumerable<IModule> modules)
        {
            _modules = modules.VerifyReferenceAndSet(nameof(modules));
        }

        public IEnumerable<IManagedComponent> GetManagedComponents()
        {
            foreach (var module in _modules)
            {
                var components = module.GetManagedComponents();

                foreach (var component in components)
                {
                    yield return component;
                }
            }
        }
    }
}