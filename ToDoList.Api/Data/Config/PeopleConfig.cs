using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Data.Config
{
    public class PeopleConfig : IEntityTypeConfiguration<People>
    {
        public void Configure(EntityTypeBuilder<People> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.MobileNumber).HasMaxLength(11);
            builder.Property(p => p.Email).HasMaxLength(100);

            builder.HasKey(p => p.Id);


        }
    }
}
