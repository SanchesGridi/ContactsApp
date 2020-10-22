using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactsApp.Infrastructure.Data.Sql.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Login).HasDefaultValueSql("CAST(NEWID() AS VARCHAR(40))");

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasAlternateKey(u => u.Email);

            builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            builder.HasMany(u => u.Contacts).WithOne(c => c.User).HasForeignKey(c => c.UserId);
            builder.HasMany(u => u.Notes).WithOne(n => n.User).HasForeignKey(n => n.UserId);
        }
    }
}

// Old Solution

// builder.Property(u => u.Login).HasDefaultValue("_x_");
// builder.HasIndex(u => new { u.Email, u.Login }).IsUnique();
// builder.HasAlternateKey(u => new { u.Email, u.Login });