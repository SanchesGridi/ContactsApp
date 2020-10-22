using System.Windows;

using GalaSoft.MvvmLight;

namespace ContactsApp.UI.UiServices
{
    public class WindowStarter : IWindowStarter
    {
        private readonly Window _owner;

        public WindowStarter()
        {
            _owner = Application.Current.MainWindow;
        }

        public bool StartDialog<TWindow>(Window owner = null) where TWindow : Window, new()
        {
            var window = new TWindow { Owner = owner ?? _owner };
            var refResult = window.ShowDialog();
            var valueResult = refResult == null ? false : refResult.Value;

            return valueResult;
        }
        public (bool result, TViewModel context) StartDialogAndGetContext<TWindow, TViewModel>(Window owner = null, string title = null)
            where TWindow : Window, new()
            where TViewModel : ViewModelBase
        {
            var window = new TWindow 
            {
                Owner = owner ?? _owner
            };
            if (!string.IsNullOrWhiteSpace(title))
            {
                window.Title = title;
            }
            var refResult = window.ShowDialog();
            var valueResult = refResult == null ? false : refResult.Value;
            var context = window.DataContext as TViewModel;

            return (result: valueResult, context: context);
        }
        public void Start<TWindow>(Window owner = null) where TWindow : Window, new()
        {
            var window = owner == null ? new TWindow() : new TWindow { Owner = owner };
            window.Show();
        }
    }
}