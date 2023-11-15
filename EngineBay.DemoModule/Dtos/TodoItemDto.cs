namespace EngineBay.DemoModule
{
    public class TodoItemDto
    {
        public TodoItemDto(TodoItem todoItem)
        {
            if (todoItem is null)
            {
                throw new ArgumentNullException(nameof(todoItem));
            }

            this.Id = todoItem.Id;
            this.Name = todoItem.Name;
            this.ListId = todoItem.ListId;
            this.Completed = todoItem.Completed;
            this.Description = todoItem.Description;
            this.DueDate = todoItem.DueDate;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ListId { get; set; }

        public bool Completed { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset? DueDate { get; set; }
    }
}