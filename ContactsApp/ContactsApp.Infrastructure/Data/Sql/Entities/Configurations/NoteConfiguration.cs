using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsApp.Infrastructure.Data.Sql.Entities.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasMany(n => n.ToDoEntries).WithOne(e => e.Note).HasForeignKey(e => e.NoteId);
        }
    }
}