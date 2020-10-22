using System.Windows;

using GalaSoft.MvvmLight;

namespace ContactsApp.UI.UiServices
{
    public interface IWindowStarter
    {
        bool StartDialog<TWindow>(Window owner = null) where TWindow : Window, new();
        (bool result, TViewModel context) StartDialogAndGetContext<TWindow, TViewModel>(Window owner = null, string title = null)
            where TWindow : Window, new()
            where TViewModel : ViewModelBase;
        void Start<TWindow>(Window owner = null) where TWindow : Window, new();
    }
}