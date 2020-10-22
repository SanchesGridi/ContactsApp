using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ContactsApp.Domain.Data.Entities;

namespace ContactsApp.Infrastructure.Data.Sql.Entities
{
    public class Note : IAuditableEntity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<ToDoEntry> ToDoEntries { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Note()
        {
            ToDoEntries = new List<ToDoEntry>();
        }
    }
}