using Autofac.Core;

using System.Collections.Generic;

namespace ContactsApp.UI.IoC.Infra
{
    public interface IManagedComponent : IComponent
    {
        IBindingInformation Info { get; }

        bool WithCtorArgs { get; }
        IEnumerable<Parameter> GetCtorArgs();
        void SetCtorArgs(IEnumerable<Parameter> arguments);
    }
}