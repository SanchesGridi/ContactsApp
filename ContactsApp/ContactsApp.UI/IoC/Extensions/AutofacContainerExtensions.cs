using Autofac;
using ContactsApp.UI.IoC.Infra;

namespace ContactsApp.UI.IoC.Extensions
{
    public static class AutofacContainerExtensions
    {
        public static void RegisterWithCtorArgs(this ContainerBuilder @this, IManagedComponent component)
        {
            var target = component.GetTarget();
            var classType = target.Key;
            var interfaceType = target.Value;

            var args = component.GetCtorArgs();

            if (component.Info.RegistrationFlag.IsPerDependency())
            {
                @this.RegisterType(classType).As(interfaceType).WithParameters(args).InstancePerDependency();
            }
            else if (component.Info.RegistrationFlag.IsSingleton())
            {
                @this.RegisterType(classType).As(interfaceType).WithParameters(args).SingleInstance();
            }
            else if (component.Info.RegistrationFlag.IsSelf())
            {
                @this.RegisterType(classType).WithParameters(args).AsSelf();
            }
        }
        public static void RegisterArgsEmpty(this ContainerBuilder @this, IManagedComponent component)
        {
            var target = component.GetTarget();
            var classType = target.Key;
            var interfaceType = target.Value;

            if (component.Info.RegistrationFlag.IsPerDependency())
            {
                @this.RegisterType(classType).As(interfaceType).InstancePerDependency();
            }
            else if (component.Info.RegistrationFlag.IsSingleton())
            {
                @this.RegisterType(classType).As(interfaceType).SingleInstance();
            }
            else if (component.Info.RegistrationFlag.IsSelf())
            {
                @this.RegisterType(classType).AsSelf();
            }
        }
    }
}