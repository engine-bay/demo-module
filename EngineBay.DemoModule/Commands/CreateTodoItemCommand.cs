namespace EngineBay.DemoModule
{
    public class CreateTodoItemCommand
    {
        public CreateTodoItemCommand(string name, Guid listId)
        {
            this.Name = name;
            this.ListId = listId;
        }

        public CreateTodoItemCommand()
        {
            this.Name = string.Empty;
            this.ListId = Guid.Empty;
        }

        public string Name { get; set; }

        public Guid ListId { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset? DueDate { get; set; }
    }
}