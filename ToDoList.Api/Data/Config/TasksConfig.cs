using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Data.Config
{
    public class TaskConfig : IEntityTypeConfiguration<Tasks>
    {
        public void Configure(EntityTypeBuilder<Tasks> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(300).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(400).IsRequired();



            builder.HasOne(e => e.Project)
                            .WithMany(e => e.TasksList)
                            .HasForeignKey(e => e.ProjectId);


            
        }
    }
}
