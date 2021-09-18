namespace Exemplum.WebApp.Features.TodoLists
{
    using Refit;
    using System.Threading.Tasks;

    public interface ITodoClient
    {
        [Get("/api/todolist")]
        Task<PaginatedList<TodoList>> GetTodoLists(int pageNumber = 1, int pageSize = 10);
        
        [Get("/api/todolist/{listId}")]
        Task<TodoList> GetTodoList(int listId = 1);
        
        [Get("/api/todolist/{listid}/todo")]
        Task<PaginatedList<TodoItem>> GetTodoItemsInList(int listId, int pageNumber = 1, int pageSize = 10);
    }
}