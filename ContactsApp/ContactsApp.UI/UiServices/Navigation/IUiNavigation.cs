using GalaSoft.MvvmLight;
using System.Windows.Controls;

namespace ContactsApp.UI.UiServices.Navigation
{
    public interface IUiNavigation
    {
        IUiNavigation InitializeFromPage<TPage>() where TPage : Page;
        void NavigateToPage<TPage>() where TPage : Page, new();
        void NavigateFromFrame<TPage>(Frame frame) where TPage : Page, new();
        TViewModel NavigateToPageAndGetContext<TPage, TViewModel>()
            where TPage : Page, new()
            where TViewModel : ViewModelBase;
        TViewModel NavigateFromFrameAndGetContext<TPage, TViewModel>(Frame frame)
            where TPage : Page, new()
            where TViewModel : ViewModelBase;
        (TViewModel context, TPage page) NavigateFromFrameAndGetData<TPage, TViewModel>(Frame frame)
            where TPage : Page, new()
            where TViewModel : ViewModelBase;
    }
}