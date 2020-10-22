using System;
using System.Windows;
using System.Collections.Generic;

using ContactsApp.UI.Models;
using ContactsApp.UI.ViewModels;
using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.UiServices
{
    public class ApplicationResourceService : IResourceService
    {
        private readonly string _userKey;
        private readonly string _noteKey;
        private readonly IEnumerable<NoteSourceViewModel> _registrationNotes;

        private ResourceDictionary Resources
        {
            get => Application.Current.Resources;
        }

        public ApplicationResourceService()
        {
            _userKey = "CurrentOwner";
            _noteKey = "CurrentOwnerNotes";
            _registrationNotes = Array.Empty<NoteSourceViewModel>();
        }

        public void SetResource<TRes>(string resourceKey, TRes resource, bool isNew = false) where TRes : class
        {
            if (isNew)
            {
                if (Resources[resourceKey] == null)
                {
                    Resources.Add(resourceKey, resource);
                }
                else
                {
                    throw new InvalidOperationException($"Resource: [{resourceKey}] not empty!");
                }
            }
            else
            {
                Resources[resourceKey] = resource;
            }
        }
        public void RemoveResource(string resourceKey)
        {
            if (Resources[resourceKey] != null)
            {
                Resources.Remove(resourceKey);
            }
        }
        public TRes GetResource<TRes>(string resourceKey) where TRes : class
        {
            try
            {
                var resourceObject = Resources[resourceKey].AsRef<TRes>();

                return resourceObject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetUserResource(UserModel user)
        {
            user.VerifyReference();

            this.SetResource(_userKey, user);
        }
        public void ResetUserResource()
        {
            this.RemoveResource(_userKey);
        }
        public UserModel GetUserResource()
        {
            return this.GetResource<UserModel>(_userKey);
        }

        public void SetUserNotesResource(IEnumerable<NoteSourceViewModel> notes = null)
        {
            this.SetResource(_noteKey, notes ?? _registrationNotes);
        }
        public void ResetUserNotesResource()
        {
            this.RemoveResource(_noteKey);
        }
        public IEnumerable<NoteSourceViewModel> GetUserNotesResource()
        {
            return this.GetResource<IEnumerable<NoteSourceViewModel>>(_noteKey);
        }
    }
}