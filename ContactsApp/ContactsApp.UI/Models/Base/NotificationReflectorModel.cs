using System;

using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.Models.Base
{
    public abstract class NotificationReflectorModel : NotificationModel
    {
        public ModelReflectElement Reflector { get; protected set; }

        public event CommandChangedEventHandler CanExecuteCommandChanged;

        public virtual void OnCanExecuteCommandChanged(string commandName)
        {
            base.Read(CanExecuteCommandChanged)?.Invoke(this, commandName);
        }

        protected internal virtual string GetKeyCommand(string key)
        {
            key.VerifyString(nameof(key), nameof(NotificationModel));

            var result = Reflector.PropertyTable.CheckStringKey(key);

            if (!result)
            {
                throw new ArgumentOutOfRangeException();
            }

            var propertyName = Reflector.PropertyTable[key];

            return propertyName;
        }
    }
}