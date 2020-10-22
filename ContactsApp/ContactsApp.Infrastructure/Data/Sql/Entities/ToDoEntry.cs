using System.ComponentModel.DataAnnotations;

using ContactsApp.Domain.Data.Entities;

namespace ContactsApp.Infrastructure.Data.Sql.Entities
{
    public class ToDoEntry : IAuditableEntity
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public int NoteId { get; set; }
        public virtual Note Note { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}