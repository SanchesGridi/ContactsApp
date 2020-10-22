using System;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;

using ContactsApp.UI.UiServices;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.UI.UiServices.Navigation;
using ContactsApp.UI.UiServices.Messangers;

namespace ContactsApp.UI.ViewModels.Base
{
    public abstract class UiMarshalingViewModel : BindingViewModel
    {
        private readonly Application _app;

        public CancellationTokenSource Source { get; private set; }

        public UiMarshalingViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService)
            : base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
        {
            _app = Application.Current;
        }

        public virtual void ReinitializeSource()
        {
            Source = new CancellationTokenSource();
        }
        public virtual void SourceCancel()
        {
            Source?.Cancel();
        }
        public virtual void MarshalAction(Action action)
        {
            base.Verify(action);

            _app.Dispatcher.Invoke(action);
        }
        public virtual Task MarshalActionAsync(Action action)
        {
            base.Verify(action);

            return _app.Dispatcher.Invoke(() => Task.Run(action));
        }
        public virtual void MarshalTaskFunction<TAny>(Func<TAny, Task> taskFunc, TAny argument)
        {
            base.Verify(taskFunc);

            _app.Dispatcher.Invoke(async () => await taskFunc.Invoke(argument));
        }
    }
}