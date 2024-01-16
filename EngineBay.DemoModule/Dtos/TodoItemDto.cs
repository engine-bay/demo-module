namespace EngineBay.DemoModule
{
    public class TodoItemDto
    {
        public TodoItemDto()
        {
            this.Id = Guid.Empty;
            this.Name = string.Empty;
            this.ListId = Guid.Empty;
            this.Completed = false;
        }

        public TodoItemDto(TodoItem todoItem)
        {
            ArgumentNullException.ThrowIfNull(todoItem);

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