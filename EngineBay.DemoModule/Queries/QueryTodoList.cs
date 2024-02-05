namespace EngineBay.DemoModule
{
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using EngineBay.Core;
    using EngineBay.Telemetry;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryTodoList : PaginatedQuery<TodoList>, IQueryHandler<DynamicFilteredPaginationParameters, PaginatedDto<TodoListDto>>
    {
        private readonly DemoModuleQueryDbContext dbContext;
        private readonly ILogger<QueryTodoList> logger;

        public QueryTodoList(DemoModuleQueryDbContext dbContext, ILogger<QueryTodoList> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<PaginatedDto<TodoListDto>> Handle(DynamicFilteredPaginationParameters query, CancellationToken cancellation)
        {
            using var activity = EngineBayActivitySource.Source.StartActivity(TracingActivityNameConstants.Handler + DemoActivityNameConstants.TodoListQuery);

            ArgumentNullException.ThrowIfNull(query);

            this.logger.QueryTodoLists();

            var lists = this.dbContext.TodoLists.AsExpandable();
            var format = new DateTimeFormatInfo();

            foreach (var filter in query.Filters)
            {
                var parameter = Expression.Parameter(typeof(TodoList));

                Expression<Func<TodoList, bool>> filterPredicate = filter.Field switch
                {
                    nameof(TodoList.Id) =>
                        filter.CreateFilterPredicate<TodoList>(parameter, Expression.Property(parameter, nameof(TodoList.Id)), Expression.Constant(Guid.Parse(filter.Value))),
                    nameof(TodoList.Name) =>
                        filter.CreateFilterPredicate<TodoList>(parameter, Expression.Property(parameter, nameof(TodoList.Name)), Expression.Constant(filter.Value)),
                    nameof(TodoList.Description) =>
                        filter.CreateFilterPredicate<TodoList>(parameter, Expression.Property(parameter, nameof(TodoList.Description)), Expression.Constant(filter.Value)),
                    nameof(TodoList.CreatedAt) =>
                        filter.CreateFilterPredicate<TodoList>(parameter, Expression.Property(parameter, nameof(TodoList.CreatedAt)), Expression.Constant(DateTime.Parse(filter.Value, format))),
                    nameof(TodoList.LastUpdatedAt) =>
                        filter.CreateFilterPredicate<TodoList>(parameter, Expression.Property(parameter, nameof(TodoList.LastUpdatedAt)), Expression.Constant(DateTime.Parse(filter.Value, format))),
                    _ => throw new ArgumentException($"TodoList Filter type {filter.Field} not found"),
                };

                lists = lists.Where(filterPredicate);
            }

            var limit = query.Limit;
            var skip = limit > 0 ? query.Skip : 0;
            var total = await lists.CountAsync(cancellation);

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
