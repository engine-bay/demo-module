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

    public class QueryTodoItem : PaginatedQuery<TodoItem>, IQueryHandler<DynamicFilteredPaginationParameters, PaginatedDto<TodoItemDto>>
    {
        private readonly DemoModuleQueryDbContext dbContext;
        private readonly ILogger<QueryTodoItem> logger;

        public QueryTodoItem(DemoModuleQueryDbContext dbContext, ILogger<QueryTodoItem> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<PaginatedDto<TodoItemDto>> Handle(DynamicFilteredPaginationParameters query, CancellationToken cancellation)
        {
            using var activity = EngineBayActivitySource.Source.StartActivity(TracingActivityNameConstants.Handler + DemoActivityNameConstants.TodoItemCreate);

            ArgumentNullException.ThrowIfNull(query);

            this.logger.QueryTodoItems();

            var items = this.dbContext.TodoItems.AsExpandable();
            var format = new DateTimeFormatInfo();

            foreach (var filter in query.Filters)
            {
                var parameter = Expression.Parameter(typeof(TodoItem));

                Expression<Func<TodoItem, bool>> filterPredicate = filter.Field switch
                {
                    nameof(TodoItem.Id) =>
                        filter.CreateFilterPredicate<TodoItem>(parameter, Expression.Property(parameter, nameof(TodoItem.Id)), Expression.Constant(Guid.Parse(filter.Value))),
                    nameof(TodoItem.Name) =>
                        filter.CreateFilterPredicate<TodoItem>(parameter, Expression.Property(parameter, nameof(TodoItem.Name)), Expression.Constant(filter.Value)),
                    nameof(TodoItem.ListId) =>
                        filter.CreateFilterPredicate<TodoItem>(parameter, Expression.Property(parameter, nameof(TodoItem.ListId)), Expression.Constant(Guid.Parse(filter.Value))),
                    nameof(TodoItem.Description) =>
                        filter.CreateFilterPredicate<TodoItem>(parameter, Expression.Property(parameter, nameof(TodoItem.Description)), Expression.Constant(filter.Value)),
                    nameof(TodoItem.Completed) =>
                        filter.CreateFilterPredicate<TodoItem>(parameter, Expression.Property(parameter, nameof(TodoItem.Completed)), Expression.Constant(bool.Parse(filter.Value))),
                    nameof(TodoItem.DueDate) =>
                        filter.CreateFilterPredicate<TodoItem>(parameter, Expression.Property(parameter, nameof(TodoItem.DueDate)), Expression.Constant(DateTime.Parse(filter.Value, format))),
                    nameof(TodoItem.CreatedAt) =>
                        filter.CreateFilterPredicate<TodoItem>(parameter, Expression.Property(parameter, nameof(TodoItem.CreatedAt)), Expression.Constant(DateTime.Parse(filter.Value, format))),
                    nameof(TodoItem.LastUpdatedAt) =>
                        filter.CreateFilterPredicate<TodoItem>(parameter, Expression.Property(parameter, nameof(TodoItem.LastUpdatedAt)), Expression.Constant(DateTime.Parse(filter.Value, format))),
                    _ => throw new ArgumentException($"TodoItem Filter type {filter.Field} not found"),
                };

                items = items.Where(filterPredicate);
            }

            var limit = query.Limit;
            var skip = limit > 0 ? query.Skip : 0;
            var total = await items.CountAsync(cancellation);

            Expression<Func<TodoItem, string?>> sortByPredicate = query.SortBy switch
            {
                nameof(TodoItem.Id) => todoItem => todoItem.Id.ToString(),
                nameof(TodoItem.Name) => todoItem => todoItem.Name,
                nameof(TodoItem.ListId) => todoItem => todoItem.ListId.ToString(),
                nameof(TodoItem.Description) => todoItem => todoItem.Description,
                nameof(TodoItem.Completed) => todoItem => todoItem.Completed.ToString(),
                nameof(TodoItem.DueDate) => todoItem => todoItem.DueDate == null ? string.Empty : todoItem.DueDate.ToString(),
                nameof(TodoItem.CreatedAt) => todoItem => todoItem.CreatedAt.ToString(format),
                nameof(TodoItem.LastUpdatedAt) => todoItem => todoItem.LastUpdatedAt.ToString(format),
                _ => throw new ArgumentException($"TodoItem SortBy type {query.SortBy} not found"),
            };

            items = this.Sort(items, sortByPredicate, query);
            items = this.Paginate(items, query);

            var todoItems = limit > 0 ? await items.ToListAsync(cancellation) : new List<TodoItem>();

            var todoItemDtos = todoItems.Select(todoItem => new TodoItemDto(todoItem));
            return new PaginatedDto<TodoItemDto>(total, skip, limit, todoItemDtos);
        }
    }
}
