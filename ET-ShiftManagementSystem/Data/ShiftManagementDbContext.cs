//using ShiftManagementServises.Servises;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using ShiftMgtDbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Data
{
    public class ShiftManagementDbContext : DbContext
    {
        public ShiftManagementDbContext(DbContextOptions options) : base(options)
        {

        }
        public ShiftManagementDbContext(DbContextOptions<ShiftManagementDbContext> options, ITenantGetter tenantGetter)
        : base(options)
        {
            //tenant = tenantGetter.Tenant;
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
            //modelBuilder
            //   .Entity<Tenate>()
            //    .HasQueryFilter(a => a.TenateId == tenant)
            //    .HasData("");
        //modelBuilder.Entity<ProjectUser>()
        //    .HasOne(x => x.project)
        //    .WithMany(x => x.Projects).
        //    HasForeignKey(x => x.ProjectId);
        //modelBuilder.Entity<ProjectUser>()
        //    .HasOne(x => x.User)
        //    .WithMany(y => y.Projects).
        //    HasForeignKey(x => x.userid);
        //modelBuilder.Entity<ProjectUser>()
        //    .HasOne(u => u.project)
        //    .WithMany(p => p.ProjectUsers)
        //    .HasForeignKey(u => u.ProjectId);
        //modelBuilder.Entity<ProjectUser>()
        //    .HasOne(u => u.User)
        //    .WithMany(p => p.ProjectUsers)
        //    .HasForeignKey(u => u.UserId);

        }
        private readonly int tenant;
        //public ShiftManagementDbContext(DbContextOptions<ShiftManagementDbContext> options, ITenantGetter tenantGetter)
        //: base(options)
        //{
        //    tenant = tenantGetter.Tenant;
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .Entity<Tenate>()
        //        .HasQueryFilter(a => a.TenateId == tenant)
        //        .HasData();
        //}
        public DbSet<Project> projects { get; set; }

        public DbSet<ProjectDetail> projectDetails { get; set; }

        public DbSet<Shift> Shifts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<User> users { get; set; }

        public DbSet<Role> roles { get; set; }

        public DbSet<User_Role> usersRoles { get; set; }

        public DbSet<Doc> Docs { get; set; }

        public DbSet<TaskDetail> taskDetails { get; set; }

        public DbSet<Token> Tokens { get; set; }

        public DbSet<SREDetaile> SREDetiles { get; set; }

        public DbSet<ProjectUser> projectUsers { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Alert> alerts { get; set; }

        public DbSet<Tenate> Tenates { get; set; }

        public DbSet<Organization> Organizations { get; set; }


    }
}
