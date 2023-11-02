namespace EngineBay.DemoModule
{
    using System;
    using EngineBay.Core;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class TodoList : BaseModel
    {
        public TodoList(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public string? Description { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<TodoList>().ToTable(typeof(TodoList).Name.Pluralize());

            modelBuilder.Entity<TodoList>().HasKey(x => x.Id);

            modelBuilder.Entity<TodoList>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<TodoList>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<TodoList>().Property(x => x.Name).IsRequired();
        }
    }
}