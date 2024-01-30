namespace EngineBay.DemoModule
{
    // High Performance logging extensions
    // https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging
    internal static class LoggerExtensions
    {
        private static readonly Action<ILogger, Guid, Exception?> NewTodoItemValue = LoggerMessage.Define<Guid>(
            logLevel: LogLevel.Debug,
            eventId: 1, // NB these eventIds must be unique per namespace
            formatString: "Created Todo Item with ID {Id}");

        private static readonly Action<ILogger, Guid, Exception?> NewTodoListValue = LoggerMessage.Define<Guid>(
            logLevel: LogLevel.Debug,
            eventId: 2,
            formatString: "Created Todo List with ID {Id}");

        private static readonly Action<ILogger, Guid, Exception?> DeleteTodoItemValue = LoggerMessage.Define<Guid>(
            logLevel: LogLevel.Debug,
            eventId: 3,
            formatString: "Deleting Todo Item with ID {Id}");

        private static readonly Action<ILogger, Guid, Exception?> DeleteTodoListValue = LoggerMessage.Define<Guid>(
            logLevel: LogLevel.Debug,
            eventId: 4,
            formatString: "Deleting Todo List with ID {Id}");

        private static readonly Action<ILogger, Guid, Exception?> UpdateTodoItemValue = LoggerMessage.Define<Guid>(
            logLevel: LogLevel.Debug,
            eventId: 5,
            formatString: "Update Todo Item with ID {Id}");

        private static readonly Action<ILogger, Guid, Exception?> UpdateTodoListValue = LoggerMessage.Define<Guid>(
            logLevel: LogLevel.Debug,
            eventId: 6,
            formatString: "Update Todo List with ID {Id}");

        private static readonly Action<ILogger, Guid, Exception?> GetTodoItemValue = LoggerMessage.Define<Guid>(
            logLevel: LogLevel.Debug,
            eventId: 7,
            formatString: "Get Todo Item with ID {Id}");

        private static readonly Action<ILogger, Guid, Exception?> GetTodoListValue = LoggerMessage.Define<Guid>(
            logLevel: LogLevel.Debug,
            eventId: 8,
            formatString: "Get Todo List with ID {Id}");

        private static readonly Action<ILogger, Exception?> QueryTodoItemsValue = LoggerMessage.Define(
            logLevel: LogLevel.Debug,
            eventId: 9,
            formatString: "Query Todo Items");

        private static readonly Action<ILogger, Exception?> QueryTodoListsValue = LoggerMessage.Define(
            logLevel: LogLevel.Debug,
            eventId: 10,
            formatString: "Query Todo Lists");

        public static void NewTodoItem(this ILogger logger, Guid id)
        {
            NewTodoItemValue(logger, id, null);
        }

        public static void NewTodoList(this ILogger logger, Guid id)
        {
            NewTodoListValue(logger, id, null);
        }

        public static void DeleteTodoItem(this ILogger logger, Guid id)
        {
            DeleteTodoItemValue(logger, id, null);
        }

        public static void DeleteTodoList(this ILogger logger, Guid id)
        {
            DeleteTodoListValue(logger, id, null);
        }

        public static void UpdateTodoItem(this ILogger logger, Guid id)
        {
            UpdateTodoItemValue(logger, id, null);
        }

        public static void UpdateTodoList(this ILogger logger, Guid id)
        {
            UpdateTodoListValue(logger, id, null);
        }

        public static void GetTodoItem(this ILogger logger, Guid id)
        {
            GetTodoItemValue(logger, id, null);
        }

        public static void GetTodoList(this ILogger logger, Guid id)
        {
            GetTodoListValue(logger, id, null);
        }

        public static void QueryTodoItems(this ILogger logger)
        {
            QueryTodoItemsValue(logger, null);
        }

        public static void QueryTodoLists(this ILogger logger)
        {
            QueryTodoListsValue(logger, null);
        }
    }
}