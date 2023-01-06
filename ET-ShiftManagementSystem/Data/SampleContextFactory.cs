using Microsoft.EntityFrameworkCore.Design;
using ShiftMgtDbContext.Data;
using Microsoft.EntityFrameworkCore;


namespace ET_ShiftManagementSystem.Data
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
