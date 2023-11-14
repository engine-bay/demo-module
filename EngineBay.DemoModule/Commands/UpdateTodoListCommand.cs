namespace EngineBay.DemoModule
{
    public class UpdateTodoListCommand
    {
        public UpdateTodoListCommand(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }
    }
}