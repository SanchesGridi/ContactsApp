using System;
using System.Collections.Generic;

namespace ContactsApp.UI.IoC.Infra
{
    public interface IComponent
    {
        KeyValuePair<Type, Type> GetTarget();
    }
}