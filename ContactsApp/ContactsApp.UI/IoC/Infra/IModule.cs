using System.Collections.Generic;

namespace ContactsApp.UI.IoC.Infra
{
    public interface IModule
    {
        IEnumerable<IManagedComponent> GetManagedComponents();
    }
}