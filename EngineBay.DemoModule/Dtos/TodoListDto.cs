namespace EngineBay.DemoModule
{
    using System.Collections.Immutable;

    public class TodoListDto
    {
        public TodoListDto()
        {
            this.Id = Guid.Empty;
            this.Name = string.Empty;
        }

        public TodoListDto(TodoList todoList)
        {
            ArgumentNullException.ThrowIfNull(todoList);

            this.Id = todoList.Id;
            this.Name = todoList.Name;
            this.Description = todoList.Description;
            this.TodoItems = todoList.TodoItems?.Select(x => new TodoItemDto(x)).ToImmutableList();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public ImmutableList<TodoItemDto>? TodoItems { get; set; }
    }
}