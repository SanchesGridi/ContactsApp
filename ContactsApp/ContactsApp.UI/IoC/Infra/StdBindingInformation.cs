namespace ContactsApp.UI.IoC.Infra
{
    public readonly struct StdBindingInformation : IBindingInformation
    {
        private readonly RegistrationFlag _registrationFlag;

        public readonly RegistrationFlag RegistrationFlag 
        { 
            get => _registrationFlag; 
        }

        public StdBindingInformation(RegistrationFlag rFlag)
        {
            _registrationFlag = rFlag;
        }
    }
}