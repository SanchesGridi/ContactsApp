using ContactsApp.UI.IoC.Infra;

namespace ContactsApp.UI.IoC.Extensions
{
    public static class IocTypesExtensions
    {
        public static bool IsPerDependency(this RegistrationFlag @this)
        {
            return @this.Equals(RegistrationFlag.PerDependency);
        }
        public static bool IsSingleton(this RegistrationFlag @this)
        {
            return @this.Equals(RegistrationFlag.Singleton);
        }
        public static bool IsSelf(this RegistrationFlag @this)
        {
            return @this.Equals(RegistrationFlag.SelfRegistration);
        }
    }
}