namespace EngineBay.DemoModule.Endpoints
{
    using EngineBay.Core;
    using EngineBay.DemoModule.Queries;

    public static class TodoListEndpoints
    {
        public const string ListBasePath = "/lists";

        private static readonly string[] TodoListTags = { ApiGroupNameConstants.TodoList };

        public static void MapEndpoints(RouteGroupBuilder endpoints)
        {
            endpoints.MapPost(
                ListBasePath,
                async (CreateTodoList handler, CreateTodoListDto createTodoListDto, CancellationToken cancellation) =>
                {
                    var result = await handler.Handle(createTodoListDto, cancellation);
                    return Results.Created($"{ListBasePath}/{result.Id}", result);
                })
                .WithTags(TodoListTags);

            endpoints.MapGet(
                ListBasePath,
                async (QueryTodoList query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, CancellationToken cancellation) =>
                {
                    var paginationParameters = new PaginationParameters(skip, limit, sortBy, sortOrder);
                    var queryTodoListRequest = new QueryTodoListRequest(paginationParameters);

                    var paginatedDtos = await query.Handle(queryTodoListRequest, cancellation);
                    return Results.Ok(paginatedDtos);
                })
                .WithTags(TodoListTags);

            endpoints.MapGet(
                ListBasePath + "/{id:guid}",
                async (GetTodoList handler, Guid id, CancellationToken cancellation) =>
                {
                    var result = await handler.Handle(id, cancellation);
                    return Results.Ok(result);
                })
                .WithTags(TodoListTags);

            endpoints.MapPut(
                ListBasePath + "/{id:guid}",
                async (UpdateTodoList handler, CreateTodoListDto createTodoListDto, Guid id, CancellationToken cancellation) =>
                {
                    var command = new UpdateTodoListCommand(id, createTodoListDto.Name)
                    {
                        Description = createTodoListDto.Description,
                    };
                    var result = await handler.Handle(command, cancellation);
                    return Results.Ok(result);
                })
                .WithTags(TodoListTags);
        }
    }
}
