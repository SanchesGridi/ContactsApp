using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ContactsApp.Domain.Data.Entities;

namespace ContactsApp.Infrastructure.Data.Sql.Entities
{
    public class Contact : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public byte[] ImageData { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<ContactSnAccount> ContactSnAccounts { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Contact()
        {
            ContactSnAccounts = new List<ContactSnAccount>();
        }
    }
}