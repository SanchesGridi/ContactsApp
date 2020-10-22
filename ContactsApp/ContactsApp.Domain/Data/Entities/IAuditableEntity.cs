using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Domain.Data.Entities
{
    public interface IAuditableEntity : IBaseEntity
    {
        [Timestamp]
        byte[] Timestamp { get; set; }
    }
}