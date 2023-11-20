namespace EngineBay.DemoModule
{
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryTodoItem : PaginatedQuery<TodoItem>, IQueryHandler<DynamicFilteredPaginationParameters, PaginatedDto<TodoItemDto>>
    {
        private readonly DemoModuleQueryDbContext dbContext;

        public QueryTodoItem(DemoModuleQueryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PaginatedDto<TodoItemDto>> Handle(DynamicFilteredPaginationParameters query, CancellationToken cancellation)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var items = this.dbContext.TodoItems.AsExpandable();
            var format = new DateTimeFormatInfo();

            foreach (var filter in query.Filters)
            {
                // Equals only
                // Expression<Func<TodoItem, bool>> filterPredicate = filter.Field switch
                // {
                //    nameof(TodoItem.Id) => todoItem => todoItem.Id == Guid.Parse(filter.Value),
                //    nameof(TodoItem.Name) => todoItem => todoItem.Name == filter.Value,
                //    nameof(TodoItem.ListId) => todoItem => todoItem.ListId == Guid.Parse(filter.Value),
                //    nameof(TodoItem.Description) => todoItem => todoItem.Description == filter.Value,
                //    nameof(TodoItem.Completed) => todoItem => todoItem.Completed == bool.Parse(filter.Value),
                //    nameof(TodoItem.DueDate) => todoItem => todoItem.DueDate == null ? filter.Value.IsNullOrEmpty() : todoItem.DueDate == DateTimeOffset.Parse(filter.Value, format),
                //    nameof(TodoItem.CreatedAt) => todoItem => todoItem.CreatedAt == DateTimeOffset.Parse(filter.Value, format),
                //    nameof(TodoItem.LastUpdatedAt) => todoItem => todoItem.LastUpdatedAt == DateTimeOffset.Parse(filter.Value, format),
                //    _ => throw new ArgumentException($"TodoItem Filter type {filter.Field} not found"),
                // };

                // Attempt 1
                // Expression<Func<TodoItem, bool>> filterPredicate = filter.CreateFilterPredicate<TodoItem>();

                // Attempt 2
                // var filterProperty = typeof(TodoItem).GetProperty(filter.Field);
                // if (filterProperty != null)
                // {
                //    Expression<Func<TodoItem, bool>> filterPredicate = filter.CreateFilterPredicate<TodoItem>(filterProperty);
                //    items = items.Where(filterPredicate);
                // }
                // else
                // {
                //    throw new ArgumentException($"TodoItem Filter type {filter.Field} not found");
                // }

                // Attempt 3 (verbose)
                // Expression<Func<TodoItem, bool>> filterPredicate;
                // switch (filter.Field)
                // {
                //    case nameof(TodoItem.Id):
                //        var comparison = filter.GetOperatorComparison<Guid>();
                //        filterPredicate = todoItem => comparison(todoItem.Id, Guid.Parse(filter.Value));
                //        break;
                //    case nameof(TodoItem.Name):
                //        var comparison = filter.GetOperatorComparison<string>();
                //        filterPredicate = todoItem => comparison(todoItem.Name, filter.Value);
                //        break;
                //    default:
                //        throw new ArgumentException($"TodoItem Filter type {filter.Field} not found");
                // }

                // Attempt 3
                // Expression<Func<TodoItem, bool>> filterPredicate = filter.Field switch
                // {
                //    nameof(TodoItem.Id) =>
                //        todoItem =>
                //            filter.GetOperatorComparison<Guid>()(todoItem.Id, Guid.Parse(filter.Value)),
                //    nameof(TodoItem.Name) =>
                //        todoItem =>
                //            filter.GetOperatorComparison<string>()(todoItem.Name, filter.Value),
                //    nameof(TodoItem.ListId) =>
                //        todoItem =>
                //            filter.GetOperatorComparison<Guid>()(todoItem.ListId, Guid.Parse(filter.Value)),
                //    nameof(TodoItem.Description) =>
                //        todoItem =>
                //            string.IsNullOrEmpty(todoItem.Description) ? filter.Value.IsNullOrEmpty() :
                //            filter.GetOperatorComparison<string>()(todoItem.Description, filter.Value),
                //    nameof(TodoItem.Completed) =>
                //        todoItem =>
                //            filter.GetOperatorComparison<bool>()(todoItem.Completed, bool.Parse(filter.Value)),
                //    nameof(TodoItem.DueDate) =>
                //        todoItem =>
                //            todoItem.DueDate == null ? filter.Value.IsNullOrEmpty() :
                //            filter.GetOperatorComparison<DateTimeOffset>()(todoItem.DueDate.Value, DateTimeOffset.Parse(filter.Value, format)),
                //    nameof(TodoItem.CreatedAt) =>
                //        todoItem =>
                //            filter.GetOperatorComparison<DateTime>()(todoItem.CreatedAt, DateTime.Parse(filter.Value, format)),
                //    nameof(TodoItem.LastUpdatedAt) =>
                //        todoItem =>
                //            filter.GetOperatorComparison<DateTime>()(todoItem.LastUpdatedAt, DateTime.Parse(filter.Value, format)),
                //    _ => throw new ArgumentException($"TodoItem Filter type {filter.Field} not found"),
                // };

                // Attempt 4
                // var parameter = Expression.Parameter(typeof(TodoItem));

                // Expression<Func<TodoItem, bool>> filterPredicate = filter.Field switch
                // {
                //    nameof(TodoItem.Id) =>

                // // filter.CreateFilterPredicate<TodoItem, Guid>(Expression.Parameter(typeof(TodoItem)), typeof(TodoItem).GetProperty(nameof(TodoItem.Id)) ?? throw new ArgumentException("This shouldn't happen")),
                //        filter.CreateFilterPredicate<TodoItem, Guid>(parameter, todoItem => todoItem.Id),
                //    nameof(TodoItem.Name) =>
                //        filter.CreateFilterPredicate<TodoItem, string>(parameter, todoItem => todoItem.Name),
                //    nameof(TodoItem.ListId) =>
                //        filter.CreateFilterPredicate<TodoItem, Guid>(parameter, todoItem => todoItem.ListId),
                //    nameof(TodoItem.Description) =>
                //        filter.CreateFilterPredicate<TodoItem, string>(parameter, todoItem => todoItem.Description ?? string.Empty),
                //    nameof(TodoItem.Completed) =>
                //        filter.CreateFilterPredicate<TodoItem, bool>(parameter, todoItem => todoItem.Completed),
                //    nameof(TodoItem.DueDate) =>
                //        filter.CreateFilterPredicate<TodoItem, DateTimeOffset>(parameter, todoItem => todoItem.DueDate),
                //    nameof(TodoItem.CreatedAt) =>
                //        filter.CreateFilterPredicate<TodoItem, DateTime>(parameter, todoItem => todoItem.CreatedAt),
                //    nameof(TodoItem.LastUpdatedAt) =>
                //        filter.CreateFilterPredicate<TodoItem, DateTime>(parameter, todoItem => todoItem.LastUpdatedAt),
                //    _ => throw new ArgumentException($"TodoItem Filter type {filter.Field} not found"),
                // };

                // items = items.Where(filterPredicate);

                // Attempt 5
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
