namespace EngineBay.DemoModule
{
    public class UpdateTodoItemCommand
    {
        public UpdateTodoItemCommand(Guid id, string name, Guid listId, bool completed)
        {
            this.Id = id;
            this.Name = name;
            this.ListId = listId;
            this.Completed = completed;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Completed { get; set; }

        public Guid ListId { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
    }
}