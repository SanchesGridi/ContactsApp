using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ContactsApp.Domain.Data.Entities;

namespace ContactsApp.Infrastructure.Data.Sql.Entities
{
    public class User : IAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public byte[] ImageData { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual List<Contact> Contacts { get; set; }
        public virtual List<Note> Notes { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public User()
        {
            Contacts = new List<Contact>();
            Notes = new List<Note>();
        }
    }
}