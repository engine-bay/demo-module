namespace EngineBay.DemoModule
{
    using EngineBay.Core;

    public class DeleteTodoList : ICommandHandler<Guid, TodoListDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;

        public DeleteTodoList(DemoModuleWriteDbContext demoModuleDb)
        {
            this.demoModuleDb = demoModuleDb;
        }

        public async Task<TodoListDto> Handle(Guid id, CancellationToken cancellation)
        {
            var todoList = await this.demoModuleDb.TodoLists.FindAsync(new object[] { id }, cancellation) ?? throw new NotFoundException($"No TodoList with Id ${id} found.");

            this.demoModuleDb.TodoLists.Remove(todoList);
            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoListDto(todoList);
        }
    }
}