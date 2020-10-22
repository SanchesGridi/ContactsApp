using System;
using System.Collections.Generic;

using Autofac.Core;

using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.IoC.Infra
{
    public class StdManagedComponent : IManagedComponent
    {
        private readonly KeyValuePair<Type, Type> _target;
        private readonly IBindingInformation _info;

        private bool _withCtorArgs;
        private IEnumerable<Parameter> _args;

        public IBindingInformation Info
        {
            get => _info;
        }
        public bool WithCtorArgs
        {
            get => _withCtorArgs;
        }

        public StdManagedComponent(KeyValuePair<Type, Type> target, IBindingInformation info, bool includeInterface = true)
        {
            var emergencyClass = nameof(StdManagedComponent);

            target.Key.VerifyReferenceInClass(nameof(target.Key), emergencyClass);

            if (includeInterface)
            {
                target.Value.VerifyReferenceInClass(nameof(target.Value), emergencyClass);
            }

            _target = target;

            if (info.GetType().IsClass)
            {
                _info = info.VerifyReferenceAndSet(nameof(info));
            }
            else
            {
                _info = info;
            }

            _withCtorArgs = false;
        }

        public KeyValuePair<Type, Type> GetTarget()
        {
            return _target;
        }
        public void SetCtorArgs(IEnumerable<Parameter> arguments)
        {
            if (arguments != null)
            {
                _args = arguments;
                _withCtorArgs = true;
            }
        }
        public IEnumerable<Parameter> GetCtorArgs()
        {
            return _args;
        }
    }
}