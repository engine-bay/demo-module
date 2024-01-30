namespace EngineBay.DemoModule
{
    using EngineBay.Core;

    public class GetTodoItem : IQueryHandler<Guid, TodoItemDto>
    {
        private readonly DemoModuleQueryDbContext demoModuleDb;
        private readonly ILogger<GetTodoItem> logger;

        public GetTodoItem(DemoModuleQueryDbContext demoModuleDb, ILogger<GetTodoItem> logger)
        {
            this.demoModuleDb = demoModuleDb;
            this.logger = logger;
        }

        public async Task<TodoItemDto> Handle(Guid query, CancellationToken cancellation)
        {
            this.logger.GetTodoItem(query);

            var todoItem = await this.demoModuleDb.TodoItems.FindAsync(new object[] { query }, cancellation) ?? throw new NotFoundException($"No TodoItem with Id ${query} found.");

            return new TodoItemDto(todoItem);
        }
    }
}
