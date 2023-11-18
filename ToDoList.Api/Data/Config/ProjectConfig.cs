using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Data.Config
{
    public class ProjectConfig : IEntityTypeConfiguration<Projects>
    {
        public void Configure(EntityTypeBuilder<Projects> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(300).IsRequired();
            builder.Property(p => p.ProjectStatus).IsRequired();
            builder.Property(p => p.PriorityLevel).IsRequired();
            builder.Property(p => p.Descrption).HasMaxLength(400);

            builder.Property(x => x.CreationDate).HasMaxLength(19);
            builder.Property(x => x.ModificationDate).HasMaxLength(19);
            builder.Property(x => x.PersianDate).HasMaxLength(10);


            builder.HasKey(p => p.Id);


         
           
                
        }
    }
}
