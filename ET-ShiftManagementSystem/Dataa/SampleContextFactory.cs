using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using ET_ShiftManagementSystem.Data;

namespace ET_ShiftManagementSystem.Dataa
{
    public interface IDesignTimeDbContextFactory<ShiftManagementDbContext> where ShiftManagementDbContext : DbContext
    {

    }
    public class SampleContextFactory : IDesignTimeDbContextFactory<ShiftManagementDbContext> 
    {
        public ShiftManagementDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ShiftManagementDbContext>();
            var connectionstring = configurationRoot.GetConnectionString("ProjectAPIConnectioString");
            builder.UseSqlServer(connectionstring , b => b.MigrationsAssembly("ShiftMgtDbContext"));

            return new ShiftManagementDbContext(builder.Options);
        }
    }
}
