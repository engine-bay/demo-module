namespace EngineBay.DemoModule
{
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using EngineBay.Core;
    using EngineBay.DemoModule.Queries;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryTodoList : PaginatedQuery<TodoList>, IQueryHandler<QueryTodoListRequest, PaginatedDto<TodoListDto>>
    {
        private readonly DemoModuleDbContext dbContext;

        public QueryTodoList(DemoModuleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PaginatedDto<TodoListDto>> Handle(QueryTodoListRequest query, CancellationToken cancellation)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (query.PaginationParameters is null)
            {
                throw new ArgumentException(nameof(query.PaginationParameters) + " is null");
            }

            var lists = this.dbContext.TodoLists.AsExpandable();

            var limit = query.PaginationParameters.Limit;
            var skip = limit > 0 ? query.PaginationParameters.Skip : 0;
            var total = await lists.CountAsync(cancellation);
            var format = new DateTimeFormatInfo();

            Expression<Func<TodoList, string?>> sortByPredicate = query.PaginationParameters.SortBy switch
            {
                nameof(TodoList.Id) => todoList => todoList.Id.ToString(),
                nameof(TodoList.Name) => todoList => todoList.Name,
                nameof(TodoList.Description) => todoList => todoList.Description,
                nameof(TodoList.CreatedAt) => todoList => todoList.CreatedAt.ToString(format),
                nameof(TodoList.LastUpdatedAt) => todoList => todoList.LastUpdatedAt.ToString(format),
                _ => throw new ArgumentException($"TodoList SortBy type {query.PaginationParameters.SortBy} not found"),
            };

            lists = this.Sort(lists, sortByPredicate, query.PaginationParameters);
            lists = this.Paginate(lists, query.PaginationParameters);

            var todoLists = limit > 0 ? await lists.ToListAsync(cancellation) : new List<TodoList>();

            var todoListDtos = todoLists.Select(todoList => new TodoListDto(todoList));
            return new PaginatedDto<TodoListDto>(total, skip, limit, todoListDtos);
        }
    }
}
