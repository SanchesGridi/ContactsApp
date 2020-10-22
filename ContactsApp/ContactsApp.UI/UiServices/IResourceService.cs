using System.Collections.Generic;

using ContactsApp.UI.Models;
using ContactsApp.UI.ViewModels;

namespace ContactsApp.UI.UiServices
{
    public interface IResourceService
    {
        void SetResource<TRes>(string resourceKey, TRes resource, bool isNew = false) where TRes : class;
        void RemoveResource(string resourceKey);
        TRes GetResource<TRes>(string resourceKey) where TRes : class;

        void SetUserResource(UserModel user);
        void ResetUserResource();
        UserModel GetUserResource();

        void SetUserNotesResource(IEnumerable<NoteSourceViewModel> notes = null);
        void ResetUserNotesResource();
        IEnumerable<NoteSourceViewModel> GetUserNotesResource();
    }
}