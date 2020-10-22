using System.Windows;

namespace ContactsApp.UI.UiServices.Messangers
{
    public interface IMessageBoxService
    {
        MessageBoxResult Go(string text);
        MessageBoxResult Go(string text, string caption);
        MessageBoxResult Go(string text, string caption, MessageBoxButton messageBoxButtons, MessageBoxImage messageBoxImage);
        MessageBoxResult Go(string text, string caption, MessageBoxButton messageBoxButtons, MessageBoxImage messageBoxImage, MessageBoxResult defaultResult);
    }
}