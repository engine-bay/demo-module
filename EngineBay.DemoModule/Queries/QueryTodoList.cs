namespace EngineBay.DemoModule
{
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryTodoList : PaginatedQuery<TodoList>, IQueryHandler<PaginationParameters, PaginatedDto<TodoListDto>>
    {
        private readonly DemoModuleQueryDbContext dbContext;

        public QueryTodoList(DemoModuleQueryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PaginatedDto<TodoListDto>> Handle(PaginationParameters query, CancellationToken cancellation)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var lists = this.dbContext.TodoLists.AsExpandable();

            var limit = query.Limit;
            var skip = limit > 0 ? query.Skip : 0;
            var total = await lists.CountAsync(cancellation);
            var format = new DateTimeFormatInfo();

            Expression<Func<TodoList, string?>> sortByPredicate = query.SortBy switch
            {
                nameof(TodoList.Id) => todoList => todoList.Id.ToString(),
                nameof(TodoList.Name) => todoList => todoList.Name,
                nameof(TodoList.Description) => todoList => todoList.Description,
                nameof(TodoList.CreatedAt) => todoList => todoList.CreatedAt.ToString(format),
                nameof(TodoList.LastUpdatedAt) => todoList => todoList.LastUpdatedAt.ToString(format),
                _ => throw new ArgumentException($"TodoList SortBy type {query.SortBy} not found"),
            };

            lists = this.Sort(lists, sortByPredicate, query);
            lists = this.Paginate(lists, query);

            var todoLists = limit > 0 ? await lists.ToListAsync(cancellation) : new List<TodoList>();

            var todoListDtos = todoLists.Select(todoList => new TodoListDto(todoList));
            return new PaginatedDto<TodoListDto>(total, skip, limit, todoListDtos);
        }
    }
}
