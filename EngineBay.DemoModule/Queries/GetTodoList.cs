namespace EngineBay.DemoModule
{
    using EngineBay.Core;

    public class GetTodoList : IQueryHandler<Guid, TodoListDto>
    {
        private readonly DemoModuleQueryDbContext demoModuleDb;

        public GetTodoList(DemoModuleQueryDbContext demoModuleDb)
        {
            this.demoModuleDb = demoModuleDb;
        }

        public async Task<TodoListDto> Handle(Guid query, CancellationToken cancellation)
        {
            var todoList = await this.demoModuleDb.TodoLists.FindAsync(new object[] { query }, cancellation) ?? throw new NotFoundException($"No TodoList with Id ${query} found.");

            return new TodoListDto(todoList);
        }
    }
}
