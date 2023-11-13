namespace EngineBay.DemoModule
{
    public class CreateOrUpdateTodoListDto
    {
        public CreateOrUpdateTodoListDto(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public string? Description { get; set; }
    }
}