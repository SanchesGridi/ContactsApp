using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.Models.Base
{
    public class ModelReflectElement
    {
        private readonly NotificationModel _instance;

        public Dictionary<string, string> PropertyTable { get; }

        public ModelReflectElement(NotificationModel instance)
        {
            _instance = instance.VerifyReferenceAndSet(nameof(instance));
            var properties = this.GetModelProperties().Select(pi => pi.Name).ToArray();

            PropertyTable = new Dictionary<string, string>();

            for (var index = 0; index < properties.Length; index++)
            {
                var fullPropertyName = $"{this.GetModelName()}_{properties[index]}";

                PropertyTable.Add(properties[index], fullPropertyName);
            }
        }

        protected internal string GetModelName(bool fullName = false)
        {
            var className = fullName ? _instance.GetType().FullName : _instance.GetType().Name;
            return className;
        }
        protected internal TypeInfo GetModelType()
        {
            return _instance.GetType().GetTypeInfo();
        }
        protected internal IEnumerable<PropertyInfo> GetModelProperties()
        {
            return this.GetModelType().DeclaredProperties;
        }
    }
}