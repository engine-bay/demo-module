namespace EngineBay.DemoModule
{
    using EngineBay.Core;

    public class DeleteTodoItem : ICommandHandler<Guid, TodoItemDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly ILogger<DeleteTodoItem> logger;

        public DeleteTodoItem(DemoModuleWriteDbContext demoModuleDb, ILogger<DeleteTodoItem> logger)
        {
            this.demoModuleDb = demoModuleDb;
            this.logger = logger;
        }

        public async Task<TodoItemDto> Handle(Guid id, CancellationToken cancellation)
        {
            this.logger.DeleteTodoItem(id);

            var todoItem = await this.demoModuleDb.TodoItems.FindAsync(new object[] { id }, cancellation) ?? throw new NotFoundException($"No TodoItem with Id ${id} found.");

            this.demoModuleDb.TodoItems.Remove(todoItem);
            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoItemDto(todoItem);
        }
    }
}