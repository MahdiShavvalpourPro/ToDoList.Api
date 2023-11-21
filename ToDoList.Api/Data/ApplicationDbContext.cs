using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDoList.Api.Data.Config;
using ToDoList.Api.Data.Entities;

namespace ToDoList.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions contextOptions) : base(contextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seed Data For People

            modelBuilder.Entity<People>()
               .HasData(
               new People()
               {
                   Id = 1,
                   FirstName = "Mahdi",
                   LastName = "Shavvalpour",
                   MobileNumber = "09398911063",
                   Email = "m.shavval@gmail.com"
               },
               new People()
               {
                   Id = 2,
                   FirstName = "Hesam",
                   LastName = "Mousavi",
                   MobileNumber = "0913256447",
                   Email = "m.Hesam@gmail.com"
               }
               , new People()
               {
                   Id = 3,
                   FirstName = "Ali",
                   LastName = "Alavi",
                   MobileNumber = "09132654478",
                   Email = "Ali@Alavi.com"
               },
               new People()
               {
                   Id = 4,
                   FirstName = "Mohsen",
                   LastName = "Mohseni",
                   MobileNumber = "09398911050",
               }
               );

            #endregion

            #region Seed Data For Project

            modelBuilder.Entity<Projects>()
                .HasData(
                new Projects()
                {
                    Id = 1,
                    Name = "نوشتن یک مقاله در رابطه با DDD",
                    PriorityLevel = PriorityLevel.Medium,
                    ProjectStatus = Status.InProgress,
                    Descrption = "این پروژه برای دانشگاه است . باید راجبش تحقیق بشه",
                    OwnerId = 1,
                },
                new Projects()
                {
                    Id = 2,
                    Name = "To Do List Advanced",
                    PriorityLevel = PriorityLevel.High,
                    ProjectStatus = Status.InProgress,
                    Descrption = "For Asp Class",
                    OwnerId = 1,
                },
                new Projects()
                {
                    Id = 3,
                    Name = "خواندن کتاب Magic Shop",
                    PriorityLevel = PriorityLevel.Low,
                    ProjectStatus = Status.InProgress,
                    Descrption = "Reading Book",
                    OwnerId = 2,
                },
                new Projects()
                {
                    Id = 4,
                    Name = "Test",
                    PriorityLevel = PriorityLevel.Medium,
                    ProjectStatus = Status.Paused,
                    Descrption = "Test",
                    OwnerId = 4
                }
                );

            #endregion

            #region Seed Data For Task

            modelBuilder.Entity<Tasks>()
                .HasData(
                new Tasks()
                {
                    Id = 1,
                    Name = "جمع آوری دیتا برای تاریخچه به وجود آمدن این رویکرد",
                    PriorityLevel = PriorityLevel.Medium,
                    TaskStatus = Status.Planning,
                    StartTime = DateTime.Now.AddDays(1),
                    ExpireTime = DateTime.Now.AddDays(2),
                    Description = "به چه علتی چنین رویکردی ایجاد شده است ؟؟",
                    ProjectId = 1,
                },
                new Tasks()
                {
                    Id = 2,
                    Name = "تشخیص بخش های اصلی سیستم . Generan _ Core _ Support",
                    PriorityLevel = PriorityLevel.Medium,
                    TaskStatus = Status.Planning,
                    StartTime = DateTime.Now.AddDays(1),
                    ExpireTime = DateTime.Now.AddDays(3),
                    Description = "",
                    ProjectId = 2,
                },
                new Tasks()
                {
                    Id = 3,
                    Name = "ادامه کتاب Magic Shop",
                    PriorityLevel = PriorityLevel.Low,
                    TaskStatus = Status.InProgress,
                    StartTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddHours(9),
                    Description = "روزانه باید این کتاب خوانده شود . باید",
                    ProjectId = 4,
                },
                new Tasks()
                {
                    Id = 4,
                    Name = "Test",
                    PriorityLevel = PriorityLevel.High,
                    TaskStatus = Status.Canceled,
                    StartTime = DateTime.Now.AddDays(1),
                    ExpireTime = DateTime.Now.AddDays(2),
                    Description = "This Is a Test Tasks",
                    ProjectId = 4,
                }
                );

            #endregion

            #region Seed Data For UserTask

            modelBuilder.Entity<UserTasks>()
                .HasData(
                new UserTasks()
                {
                    Id = 1,
                    UserId = 1,
                    TaskId = 1,

                },
                new UserTasks()
                {
                    Id = 2,
                    UserId = 1,
                    TaskId = 2,

                },
                new UserTasks()
                {
                    Id = 3,
                    UserId = 2,
                    TaskId = 3,

                },
                new UserTasks()
                {
                    Id = 4,
                    UserId = 4,
                    TaskId = 4,
                }
                );

            #endregion

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
