namespace EngineBay.DemoModule
{
    public class CreateTodoItemDto
    {
        public CreateTodoItemDto(string name, Guid listId)
        {
            this.Name = name;
            this.ListId = listId;
        }

        public string Name { get; set; }

        public Guid ListId { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
    }
}