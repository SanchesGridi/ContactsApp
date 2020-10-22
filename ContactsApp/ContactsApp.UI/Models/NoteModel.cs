using System;
using System.Collections.ObjectModel;

using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;

namespace ContactsApp.UI.Models
{
    public class NoteModel : NotificationTokenModel
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private string _title;
        private string _description;
        private ObservableCollection<EntryModel> _entries;

        public DateTime StartDate
        {
            get => _startDate;
            set => this.SetWithCommandStateUpdate(ref _startDate, value, nameof(StartDate));
        }
        public DateTime EndDate
        {
            get => _endDate;
            set => this.SetWithCommandStateUpdate(ref _endDate, value, nameof(StartDate));
        }
        public string Title
        {
            get => _title;
            set => this.SetWithCommandStateUpdate(ref _title, value, nameof(Title));
        }
        public string Description
        {
            get => _description;
            set => this.SetWithCommandStateUpdate(ref _description, value, nameof(Description));
        }
        public ObservableCollection<EntryModel> Entries
        {
            get => _entries ?? (_entries = new ObservableCollection<EntryModel>());
            set => this.SetWithCommandStateUpdate(ref _entries, value, nameof(Entries));
        }

        public NoteModel(int token = NotExistToken) : base(token)
        {
            Reflector = new ModelReflectElement(this);
        }
    }
}