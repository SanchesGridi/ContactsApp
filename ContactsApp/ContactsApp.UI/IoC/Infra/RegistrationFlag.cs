namespace ContactsApp.UI.IoC.Infra
{
    public enum RegistrationFlag : byte
    {
        Singleton = 0,
        PerDependency = 1,
        SelfRegistration = 2
    }
}