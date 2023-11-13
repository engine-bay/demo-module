namespace EngineBay.DemoModule
{
    using System;
    using EngineBay.Core;
    using Humanizer;
    using Microsoft.EntityFrameworkCore;

    public class TodoItem : BaseModel
    {
        public TodoItem(string name, Guid listId, bool completed)
        {
            this.Name = name;
            this.ListId = listId;
            this.Completed = completed;
        }

        public string Name { get; set; }

        public Guid ListId { get; set; }

        public virtual TodoList? List { get; set; }

        public bool Completed { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset? DueDate { get; set; }

        public static new void CreateDataAnnotations(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<TodoItem>().ToTable(typeof(TodoItem).Name.Pluralize());

            modelBuilder.Entity<TodoItem>().HasKey(x => x.Id);

            modelBuilder.Entity<TodoItem>().Property(x => x.CreatedAt).IsRequired();

            modelBuilder.Entity<TodoItem>().Property(x => x.LastUpdatedAt).IsRequired();

            modelBuilder.Entity<TodoItem>().Property(x => x.Name).IsRequired();

            modelBuilder.Entity<TodoItem>().Property(x => x.ListId).IsRequired();

            modelBuilder.Entity<TodoItem>().HasOne(x => x.List).WithMany().HasForeignKey(x => x.ListId);

            modelBuilder.Entity<TodoItem>().Property(x => x.Completed).IsRequired();

            modelBuilder.Entity<TodoItem>().Property(x => x.Description);

            modelBuilder.Entity<TodoItem>().Property(x => x.DueDate);
        }
    }
}