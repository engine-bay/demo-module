namespace EngineBay.DemoModule
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class DemoModuleQueryDbContext : DemoModuleDbContext
    {
        public DemoModuleQueryDbContext(DbContextOptions<ModuleWriteDbContext> options)
            : base(options)
        {
        }
    }
}