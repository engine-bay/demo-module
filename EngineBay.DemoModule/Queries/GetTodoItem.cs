namespace EngineBay.DemoModule
{
    using EngineBay.Core;

    public class GetTodoItem : IQueryHandler<Guid, TodoItemDto>
    {
        private readonly DemoModuleQueryDbContext demoModuleDb;

        public GetTodoItem(DemoModuleQueryDbContext demoModuleDb)
        {
            this.demoModuleDb = demoModuleDb;
        }

        public async Task<TodoItemDto> Handle(Guid query, CancellationToken cancellation)
        {
            var todoItem = await this.demoModuleDb.TodoItems.FindAsync(new object[] { query }, cancellation) ?? throw new NotFoundException($"No TodoItem with Id ${query} found.");

            return new TodoItemDto(todoItem);
        }
    }
}
