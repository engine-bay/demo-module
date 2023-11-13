namespace EngineBay.DemoModule
{
    public class CreateTodoItemDto
    {
        public CreateTodoItemDto(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset? DueDate { get; set; }
    }
}