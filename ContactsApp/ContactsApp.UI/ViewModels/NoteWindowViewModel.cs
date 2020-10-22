using System.Linq;
using System.Windows.Input;
using System.Windows.Documents;

using GalaSoft.MvvmLight.Command;

using ContactsApp.UI.UiServices;
using ContactsApp.UI.WpfExtesions;
using ContactsApp.UI.Views.Windows;
using ContactsApp.UI.ViewModels.Base;
using ContactsApp.UI.UiServices.Commands;
using ContactsApp.UI.UiServices.Messangers;
using ContactsApp.UI.UiServices.Navigation;

namespace ContactsApp.UI.ViewModels
{
    public class NoteWindowViewModel : UiMarshalingViewModel
    {
        private string _titleMode;
        private NoteSourceViewModel _noteSourceVm;
        public string TitleMode
        {
            get => _titleMode;
            set => this.SetValue(ref _titleMode, value, nameof(TitleMode));
        }
        public NoteSourceViewModel NoteSourceVm
        {
            get => _noteSourceVm ?? (_noteSourceVm = new NoteSourceViewModel());
            set => this.SetValue(ref _noteSourceVm, value, nameof(_noteSourceVm));
        }
        public ICommand SaveCmd { get; }
        public ICommand CancelCmd { get; }

        public NoteWindowViewModel(
            ICommandProvider cmdProvider,
            IUiNavigation navigation,
            IMessageBoxService messanger,
            IObjectSearcher objectSearcher,
            IResourceService resourceService)
            : base(cmdProvider, navigation, messanger, objectSearcher, resourceService)
        {
            SaveCmd = new RelayCommand<NoteWindow>(this.SaveExecute);
            CancelCmd = new RelayCommand<NoteWindow>(this.CancelExecute);
        }

        private void SaveExecute(NoteWindow window) // TODO
        {
            var blocks = window.BoxWithEntries.Document.Blocks;
            var entries = blocks.Where(b => b is List list);

            foreach (var entry in entries)
            {

            }
            window.DialogResult = true;
        }
        private void CancelExecute(NoteWindow window) // TODO
        {
            window.DialogResult = false;
        }
    }
}