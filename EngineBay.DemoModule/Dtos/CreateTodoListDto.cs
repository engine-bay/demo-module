namespace EngineBay.DemoModule
{
    public class CreateTodoListDto
    {
        public CreateTodoListDto(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public string? Description { get; set; }
    }
}