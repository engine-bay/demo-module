﻿namespace EngineBay.DemoModule
{
    using EngineBay.Core;

    public static class TodoItemEndpoints
    {
        private const string ItemBasePath = "/lists/items";

        private static readonly string[] TodoItemTags = { ApiGroupNameConstants.TodoItem };

        public static void MapEndpoints(RouteGroupBuilder endpoints)
        {
            endpoints.MapPost(
                ItemBasePath,
                async (CreateTodoItem handler, CreateTodoItemDto createTodoItemDto, CancellationToken cancellation) =>
                {
                    var result = await handler.Handle(createTodoItemDto, cancellation);
                    return Results.Created($"{ItemBasePath}/{result.Id}", result);
                })
                .WithTags(TodoItemTags);

            endpoints.MapGet(
                ItemBasePath,
                async (QueryTodoItem handler, int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, string?[] filters, CancellationToken cancellation) =>
                {
                    var dynamicFilteredPaginationParameters = new DynamicFilteredPaginationParameters(skip, limit, sortBy, sortOrder, filters);

                    var paginatedDtos = await handler.Handle(dynamicFilteredPaginationParameters, cancellation);
                    return Results.Ok(paginatedDtos);
                })
                .WithTags(TodoItemTags);

            endpoints.MapGet(
                ItemBasePath + "/{id:guid}",
                async (GetTodoItem handler, Guid id, CancellationToken cancellation) =>
                {
                    var result = await handler.Handle(id, cancellation);
                    return Results.Ok(result);
                })
                .WithTags(TodoItemTags);

            endpoints.MapPut(
                ItemBasePath + "/{id:guid}",
                async (UpdateTodoItem handler, UpdateTodoItemDto updateTodoItemDto, Guid id, CancellationToken cancellation) =>
                {
                    var command = new UpdateTodoItemCommand(id, updateTodoItemDto.Name, updateTodoItemDto.ListId, updateTodoItemDto.Completed)
                    {
                        Description = updateTodoItemDto.Description,
                        DueDate = updateTodoItemDto.DueDate,
                    };
                    var result = await handler.Handle(command, cancellation);
                    return Results.Ok(result);
                })
                .WithTags(TodoItemTags);

            endpoints.MapDelete(
                ItemBasePath + "/{id:guid}",
                async (DeleteTodoItem handler, Guid id, CancellationToken cancellation) =>
                {
                    var result = await handler.Handle(id, cancellation);
                    return Results.Ok(result);
                })
                .WithTags(TodoItemTags);
        }
    }
}
