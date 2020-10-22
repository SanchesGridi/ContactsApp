namespace ContactsApp.UI.UiServices
{
    public interface IDialogService
    {
        string FilePath { get; }
        bool OpenFileDialog(string filter = "all files (*.*)|*.*");
        bool SaveFileDialog(string filter = "all files (*.*)|*.*", string defaultName = "_NewFile_");
    }
}