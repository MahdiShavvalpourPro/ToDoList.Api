using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Data.Config
{
    public class UserTasksConfig : IEntityTypeConfiguration<UserTasks>
    {
        public void Configure(EntityTypeBuilder<UserTasks> builder)
        {
            builder.HasOne(e => e.People)
               .WithMany(e => e.UserTasks)
               .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Task)
                .WithMany(e => e.UserTasks)
                .HasForeignKey(e => e.TaskId);
        }
    }
}
