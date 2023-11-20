namespace EngineBay.DemoModule.Endpoints
{
    using EngineBay.Core;

    public static class TodoListEndpoints
    {
        public const string ListBasePath = "/lists";

        private static readonly string[] TodoListTags = { ApiGroupNameConstants.TodoList };

        public static void MapEndpoints(RouteGroupBuilder endpoints)
        {
            endpoints.MapPost(
                ListBasePath,
                async (CreateTodoList handler, CreateOrUpdateTodoListDto createTodoListDto, CancellationToken cancellation) =>
                {
                    var result = await handler.Handle(createTodoListDto, cancellation);
                    return Results.Created($"{ListBasePath}/{result.Id}", result);
                })
                .WithTags(TodoListTags);

            endpoints.MapGet(
                ListBasePath,
                async (QueryTodoList query, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, string?[] filters, CancellationToken cancellation) =>
                {
                    var paginationParameters = new DynamicFilteredPaginationParameters(skip, limit, sortBy, sortOrder, filters);

                    var paginatedDtos = await query.Handle(paginationParameters, cancellation);
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
                async (UpdateTodoList handler, CreateOrUpdateTodoListDto updateTodoListDto, Guid id, CancellationToken cancellation) =>
                {
                    var command = new UpdateTodoListCommand(id, updateTodoListDto.Name)
                    {
                        Description = updateTodoListDto.Description,
                    };
                    var result = await handler.Handle(command, cancellation);
                    return Results.Ok(result);
                })
                .WithTags(TodoListTags);

            endpoints.MapDelete(
                ListBasePath + "/{id:guid}",
                async (DeleteTodoList handler, Guid id, CancellationToken cancellation) =>
                {
                    var result = await handler.Handle(id, cancellation);
                    return Results.Ok(result);
                })
                .WithTags(TodoListTags);
        }
    }
}
