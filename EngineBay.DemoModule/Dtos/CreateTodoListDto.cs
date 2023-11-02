namespace EngineBay.DemoModule
{
    public class CreateTodoListDto
    {
        public CreateTodoListDto(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}