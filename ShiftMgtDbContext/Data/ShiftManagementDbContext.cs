//using ShiftManagementServises.Servises;
using Microsoft.EntityFrameworkCore;
using ShiftMgtDbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftMgtDbContext.Data
{
    public class ShiftManagementDbContext : DbContext
    {
        public ShiftManagementDbContext(DbContextOptions options) : base(options)  
        
        {
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.Role)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.RoleId);
            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.User)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.Userid);

        }

        public DbSet<Project> projects { get; set; }

        //public DbSet<User> users { get; set; }

        public DbSet<ProjectDetail> projectDetails { get; set; }

        public DbSet<Shift> Shifts { get; set; }

       
        public DbSet<Comment> Comments { get; set; }

        // public DbSet<UserCredential> UserCredentials { get; set; }

        //public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> users { get; set; }

        public DbSet<Role> roles { get; set; }

        public DbSet<User_Role> usersRoles { get; set; }

        public DbSet<Doc> Docs { get; set; }

        public DbSet<TaskDetail> taskDetails { get; set; }

        public DbSet<Email> Emails { get; set; }

    }
}
