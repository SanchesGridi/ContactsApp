using System.ComponentModel;

namespace ContactsApp.UI.Models
{
    public interface ISocialAccountModel : INotifyPropertyChanged
    {
        string UserName { get; set; }
        string DisplayUserName { get; set; }
        string AccountIdentifierUserName { get; set; }

        string GetAccountTag();
        string GetDomainName();
    }
}