namespace EngineBay.DemoModule
{
    public class TodoListDto
    {
        public TodoListDto(TodoList todoList)
        {
            if (todoList is null)
            {
                throw new ArgumentNullException(nameof(todoList));
            }

            this.Id = todoList.Id;
            this.Name = todoList.Name;
            this.Description = todoList.Description ?? string.Empty;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}