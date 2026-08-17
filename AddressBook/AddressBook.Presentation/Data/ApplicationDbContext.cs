using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Presentation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<JobTitle> Jobs { get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Addresses)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.Department)
                .WithMany(d => d.Addresses)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            #region seed data to department
            modelBuilder.Entity<Department>().HasData(
                    new Department
                    {
                        Id = 1,
                        Name = "IT"
                    },
                    new Department
                    {
                        Id = 2,
                        Name = "HR"
                    },
                    new Department
                    {
                        Id = 3,
                        Name = "Finance"
                    },
                    new Department
                    {
                        Id = 4,
                        Name = "Marketing"
                    },
                    new Department
                    {
                        Id = 5,
                        Name = "Sales"
                    }
                );
            #endregion

            #region seed data to jobtitle
            modelBuilder.Entity<JobTitle>().HasData(
                   new JobTitle
                   {
                       Id = 1,
                       Name = "Software Engineer"
                   },
                   new JobTitle
                   {
                       Id = 2,
                       Name = "Senior Software Engineer"
                   },
                   new JobTitle
                   {
                       Id = 3,
                       Name = "Project Manager"
                   },
                   new JobTitle
                   {
                       Id = 4,
                       Name = "HR Specialist"
                   },
                   new JobTitle
                   {
                       Id = 5,
                       Name = "Accountant"
                   },
                   new JobTitle
                   {
                       Id = 6,
                       Name = "Sales Representative"
                   },
                   new JobTitle
                   {
                       Id = 7,
                       Name = "Marketing Specialist"
                   }
               );
            #endregion
        }
    }
}