namespace EngineBay.DemoModule
{
    using System;
    using EngineBay.Persistence;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class TodoList : BaseModel
    {
        public TodoList()
        {
            this.Name = string.Empty;
        }

        public TodoList(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<TodoItem>? TodoItems { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.Entity<TodoList>().ToTable(typeof(TodoList).Name.Pluralize());

            modelBuilder.Entity<TodoList>().HasKey(x => x.Id);

            modelBuilder.Entity<TodoList>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<TodoList>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<TodoList>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<TodoList>().Property(x => x.Description);

            modelBuilder.Entity<TodoList>().HasMany(x => x.TodoItems).WithOne().HasForeignKey(x => x.ListId);
        }
    }
}