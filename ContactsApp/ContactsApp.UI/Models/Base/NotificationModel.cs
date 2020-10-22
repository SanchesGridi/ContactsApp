using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ContactsApp.UI.Models.Base
{
    public abstract class NotificationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.Read(PropertyChanged)?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected internal TClass Read<TClass>(TClass @class) where TClass : class
        {
            return Volatile.Read(ref @class);
        }
    }
}