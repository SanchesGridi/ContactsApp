using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using ContactsApp.UI.Models;
using ContactsApp.UI.Models.Base;
using ContactsApp.Infrastructure.Extensions;
using ContactsApp.Infrastructure.Data.Sql.Entities;

namespace ContactsApp.UI.WpfExtesions
{
    public static class NotificationTypesExtensions
    {
        public static void SetValue<TValue>(this INotifyPropertyChanged @this, ref TValue field, TValue value, [CallerMemberName]string propertyName = null)
        {
            field = value;

            if (@this is ViewModelBase viewModel)
            {
                viewModel.RaisePropertyChanged(propertyName);
            }
            else if (@this is NotificationModel model)
            {
                model.OnPropertyChanged(propertyName);
            }
        }
        public static void RaiseCanExecuteCommandChanged(this NotificationReflectorModel @this, in string propertyIndex)
        {
            var emergencyClass = nameof(NotificationTypesExtensions);

            @this.VerifyReferenceInClass(nameof(@this), emergencyClass);
            propertyIndex.VerifyString(nameof(propertyIndex), emergencyClass);

            var propertyName = @this.Reflector.PropertyTable[propertyIndex];

            @this.OnCanExecuteCommandChanged(propertyName);
        }
        public static void SetWithCommandStateUpdate<TValue>(this NotificationReflectorModel @this, ref TValue field, TValue value, [CallerMemberName]string propertyName = null)
        {
            @this.SetValue(ref field, value, propertyName);
            @this.RaiseCanExecuteCommandChanged(propertyName);
        }
    }

    public static class RelayCommandExtensions
    {
        public static void RaiseRelayDefaultCommand(this ICommand command)
        {
            (command as RelayCommand)?.RaiseCanExecuteChanged();
        }
        public static void RaiseRelayGenericCommand<TAny>(this ICommand command)
        {
            (command as RelayCommand<TAny>)?.RaiseCanExecuteChanged();
        }
    }

    public static class ModelExtensions
    {
        public static ObservableCollection<TelegramAccountModel> GetTelegramAcoounts(this ContactModel @this)
        {
            @this.VerifyReference();

            var accounts = (IEnumerable<TelegramAccountModel>)null;

            if (@this.AccountModels.Count > 0)
            {
                accounts = @this.AccountModels.Where(x => x.GetAccountTag() == AccountDefinition.TelegramTag).Cast<TelegramAccountModel>();
            }
            else
            {
                accounts = new List<TelegramAccountModel>();
            }

            return new ObservableCollection<TelegramAccountModel>(accounts);
        }
    }

    public static class ViewModelExtensions
    {
        public static ImageSource GetSource(this ViewModelBase @this, string path)
        {
            var bytes = File.ReadAllBytes(path);

            using (var stream = new MemoryStream(bytes))
            {
                var source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                return source;
            }
        }
    }
}