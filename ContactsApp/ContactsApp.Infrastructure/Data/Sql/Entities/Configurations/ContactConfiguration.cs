using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsApp.Infrastructure.Data.Sql.Entities.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasMany(c => c.ContactSnAccounts).WithOne(a => a.OwnerContact).HasForeignKey(a => a.OwnerContactId);
        }
    }
}