using GalaSoft.MvvmLight;

using ContactsApp.UI.Models;
using ContactsApp.UI.WpfExtesions;

namespace ContactsApp.UI.ViewModels
{
    public class NoteSourceViewModel : ViewModelBase
    {
		private NoteModel _noteModel;

		public NoteModel NoteModel
		{
			get => _noteModel ?? (_noteModel = new NoteModel());
			set => this.SetValue(ref _noteModel, value, nameof(NoteModel));
		}
		public string Title
		{
			get => NoteModel.Title;
			set { NoteModel.Title = value; base.RaisePropertyChanged(); }
		}
		public string Description
		{
			get => NoteModel.Description;
			set { NoteModel.Description = value; base.RaisePropertyChanged(); }
		}
		public string StartDate
		{
			get => NoteModel.StartDate.Date.ToShortDateString();
		}
		public string EndDate
		{
			get => NoteModel.EndDate.Date.ToShortDateString();
		}
	}
}