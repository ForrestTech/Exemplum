namespace Exemplum.WebApp.Features.TodoLists.Client
{
    using Refit;
    using System.Threading.Tasks;

    public interface ITodoClient
    {
        [Get("/api/todolist")]
        Task<PaginatedList<TodoList>> GetTodoLists(int pageNumber = 1, int pageSize = 10);

        [Post("/api/todolist")]
        Task<TodoList> CreateTodoList(CreateTodoList command);

        [Get("/api/todolist/{listId}")]
        Task<TodoList> GetTodoList(int listId = 1);

        [Put("/api/todolist/{listId}")]
        Task<TodoList> UpdateTodoList(int listId, UpdateTodoList command);

        [Delete("/api/todolist/{listId}")]
        Task DeleteTodoList(int listId);

        [Get("/api/todolist/{listId}/todo")]
        Task<PaginatedList<TodoItem>> GetTodoItemsInList(int listId, int pageNumber = 1, int pageSize = 10);

        [Post("/api/todolist/{listId}/todo")]
        Task<TodoItem> CreateTodoItem(int listId, CreateTodoItem command);

        [Put("/api/todolist/{listId}/todo/{todoId}")]
        Task<TodoItem> UpdateTodoItem(int listId, int todoId, UpdateTodoItem command);
        
        [Post("/api/todolist/{listId}/todo/{todoId}/completed")]
        Task MarkTodoItemCompleted(int listId, int todoId);
        
        [Delete("/api/todolist/{listId}/todo/{todoId}")]
        Task DeleteTodoItem(int listId, int todoId);
    }
}