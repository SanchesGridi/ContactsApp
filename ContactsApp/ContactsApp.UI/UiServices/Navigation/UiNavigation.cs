using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using GalaSoft.MvvmLight;

using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.UiServices.Navigation
{
    public class UiNavigation : IUiNavigation
    {
        private bool _firstTime;
        private Lazy<NavigationService> _navigation;

        public UiNavigation()
        {
            _firstTime = true;
        }

        public IUiNavigation InitializeFromPage<TPage>() where TPage : Page
        {
            var window = Application.Current.MainWindow;
            var page = window.Content as TPage;

            if (page != null)
            {
                _navigation = new Lazy<NavigationService>(() => NavigationService.GetNavigationService(page));
                _firstTime = false;
            }

            return this;
        }
        public void NavigateFromFrame<TPage>(Frame frame) where TPage : Page, new()
        {
            frame.Navigate(new TPage());
        }
        public void NavigateToPage<TPage>() where TPage : Page, new()
        {
            if (_firstTime)
            {
                this.InitializeFromPage<Page>();
            }

            var page = new TPage();
            _navigation.Value.Navigate(page);
        }
        public TViewModel NavigateToPageAndGetContext<TPage, TViewModel>() 
            where TPage : Page, new()
            where TViewModel : ViewModelBase
        {
            if (_firstTime)
            {
                this.InitializeFromPage<Page>();
            }

            var page = new TPage();
            _navigation.Value.Navigate(page);

            var context = page.DataContext as TViewModel;
            return context.VerifyReferenceAndSet();
        }
        public TViewModel NavigateFromFrameAndGetContext<TPage, TViewModel>(Frame frame)
            where TPage : Page, new()
            where TViewModel : ViewModelBase
        {
            var page = new TPage();
            frame.Navigate(page);

            var context = page.DataContext as TViewModel;
            return context.VerifyReferenceAndSet();
        }
        public (TViewModel context, TPage page) NavigateFromFrameAndGetData<TPage, TViewModel>(Frame frame)
            where TPage : Page, new()
            where TViewModel : ViewModelBase
        {
            var page = new TPage();
            frame.Navigate(page);

            var context = page.DataContext as TViewModel;
            return (context.VerifyReferenceAndSet(), page.VerifyReferenceAndSet());
        }
    }
}