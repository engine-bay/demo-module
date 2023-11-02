namespace EngineBay.DemoModule
{
    using EngineBay.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class DemoModuleDbContext : ModuleWriteDbContext
    {
        public DemoModuleDbContext(DbContextOptions<ModuleWriteDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoList> TodoLists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            TodoList.CreateDataAnnotations(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}