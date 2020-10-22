using System.ComponentModel.DataAnnotations;

using ContactsApp.Domain.Data.Entities;

namespace ContactsApp.Infrastructure.Data.Sql.Entities
{
    public abstract class ContactSnAccount : IAuditableEntity
    {
        public int Id { get; set; }
        public string Discriminator { get; }

        public int OwnerContactId { get; set; }
        public virtual Contact OwnerContact { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public abstract string GetAccountTag();
        public abstract string GetAccountDomainName();
    }
}