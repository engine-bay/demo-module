namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using Microsoft.EntityFrameworkCore;

    public class GetTodoList : IQueryHandler<Guid, TodoListDto>
    {
        private readonly DemoModuleQueryDbContext demoModuleDb;
        private readonly ILogger<GetTodoList> logger;

        public GetTodoList(DemoModuleQueryDbContext demoModuleDb, ILogger<GetTodoList> logger)
        {
            this.demoModuleDb = demoModuleDb;
            this.logger = logger;
        }

        public async Task<TodoListDto> Handle(Guid query, CancellationToken cancellation)
        {
            this.logger.GetTodoList(query);

            var todoList = await this.demoModuleDb.TodoLists.Include(x => x.TodoItems).FirstOrDefaultAsync(x => x.Id == query, cancellation) ?? throw new NotFoundException($"No TodoList with Id ${query} found.");

            return new TodoListDto(todoList);
        }
    }
}
