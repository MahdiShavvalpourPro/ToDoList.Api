using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Data.Config
{
    public class BaseEntityConfig : IEntityTypeConfiguration<BaseEntity>
    {
        public void Configure(EntityTypeBuilder<BaseEntity> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(x => x.CreationDate).HasMaxLength(19);
            builder.Property(x => x.ModificationDate).HasMaxLength(19);
            builder.Property(x => x.PersianDate).HasMaxLength(10);

        }
    }
}
