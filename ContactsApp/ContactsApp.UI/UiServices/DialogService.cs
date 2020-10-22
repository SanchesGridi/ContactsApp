using Microsoft.Win32;

namespace ContactsApp.UI.UiServices
{
    public class DialogService : IDialogService
    {
        public string FilePath { get; private set; }

        public bool OpenFileDialog(string filter = "all files (*.*)|*.*")
        {
            var ofd = new OpenFileDialog
            {
                Filter = filter,
                Title = "Select File",
            };
            if (ofd.ShowDialog() == true)
            {
                FilePath = ofd.FileName;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SaveFileDialog(string filter = "all files (*.*)|*.*", string defaultName = "_NewFile_")
        {
            var sfd = new SaveFileDialog
            {
                Filter = filter
            };
            if (!defaultName.Equals(""))
            {
                sfd.FileName = defaultName;
            }
            if (sfd.ShowDialog() == true)
            {
                FilePath = sfd.FileName;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}