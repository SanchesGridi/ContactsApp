using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ContactsApp.Domain.Data.Entities;

namespace ContactsApp.Infrastructure.Data.Sql.Entities
{
    public class Role : IAuditableEntity
    {
        public int Id { get; set; }
        public string Access { get; set; }

        public virtual List<User> Users { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}