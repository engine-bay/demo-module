namespace EngineBay.DemoModule
{
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class QueryTodoItem : PaginatedQuery<TodoItem>, IQueryHandler<PaginationParameters, PaginatedDto<TodoItemDto>>
    {
        private readonly DemoModuleQueryDbContext dbContext;

        public QueryTodoItem(DemoModuleQueryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PaginatedDto<TodoItemDto>> Handle(PaginationParameters query, CancellationToken cancellation)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var items = this.dbContext.TodoItems.AsExpandable();

            var limit = query.Limit;
            var skip = limit > 0 ? query.Skip : 0;
            var total = await items.CountAsync(cancellation);
            var format = new DateTimeFormatInfo();

            Expression<Func<TodoItem, string?>> sortByPredicate = query.SortBy switch
            {
                nameof(TodoItem.Id) => todoList => todoList.Id.ToString(),
                nameof(TodoItem.Name) => todoList => todoList.Name,
                nameof(TodoItem.Description) => todoList => todoList.Description,
                nameof(TodoItem.Completed) => todoList => todoList.Completed.ToString(),
                nameof(TodoItem.DueDate) => todoList => todoList.DueDate == null ? string.Empty : todoList.DueDate.ToString(),
                nameof(TodoItem.CreatedAt) => todoList => todoList.CreatedAt.ToString(format),
                nameof(TodoItem.LastUpdatedAt) => todoList => todoList.LastUpdatedAt.ToString(format),
                _ => throw new ArgumentException($"TodoItem SortBy type {query.SortBy} not found"),
            };

            items = this.Sort(items, sortByPredicate, query);
            items = this.Paginate(items, query);

            var todoLists = limit > 0 ? await items.ToListAsync(cancellation) : new List<TodoItem>();

            var todoListDtos = todoLists.Select(todoList => new TodoItemDto(todoList));
            return new PaginatedDto<TodoItemDto>(total, skip, limit, todoListDtos);
        }
    }
}
