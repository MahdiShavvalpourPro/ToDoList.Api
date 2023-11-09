using Microsoft.EntityFrameworkCore;
using ToDoList.Api.Data.Config;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions contextOptions) : base(contextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PeopleConfig());
            modelBuilder.ApplyConfiguration(new TaskConfig());
            modelBuilder.ApplyConfiguration(new ProjectConfig());
            modelBuilder.ApplyConfiguration(new UserTasksConfig());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<People> Tbl_People { get; set; }
        public DbSet<Tasks> Tbl_Task { get; set; }
        public DbSet<Projects> Tbl_Project { get; set; }
        public DbSet<UserTasks> Tbl_UserTask { get; set; }

    }
}
