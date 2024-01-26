namespace EngineBay.DemoModule
{
    using EngineBay.Core;

    public class DeleteTodoList : ICommandHandler<Guid, TodoListDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly ILogger<DeleteTodoList> logger;

        public DeleteTodoList(DemoModuleWriteDbContext demoModuleDb, ILogger<DeleteTodoList> logger)
        {
            this.demoModuleDb = demoModuleDb;
            this.logger = logger;
        }

        public async Task<TodoListDto> Handle(Guid id, CancellationToken cancellation)
        {
            this.logger.DeleteTodoList(id);

            var todoList = await this.demoModuleDb.TodoLists.FindAsync(new object[] { id }, cancellation) ?? throw new NotFoundException($"No TodoList with Id ${id} found.");

            this.demoModuleDb.TodoLists.Remove(todoList);
            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoListDto(todoList);
        }
    }
}