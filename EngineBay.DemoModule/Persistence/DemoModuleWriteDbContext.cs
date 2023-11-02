namespace EngineBay.DemoModule
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class DemoModuleWriteDbContext : DemoModuleQueryDbContext
    {
        public DemoModuleWriteDbContext(DbContextOptions<ModuleWriteDbContext> options)
            : base(options)
        {
        }
    }
}