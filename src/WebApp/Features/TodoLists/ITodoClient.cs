namespace Exemplum.WebApp.Features.TodoLists
{
    using Microsoft.AspNetCore.Mvc;
    using Refit;
    using System.Threading.Tasks;

    public interface ITodoClient
    {
        [Get("/api/todolist/{listid}/todo")]
        Task<PaginatedList<TodoItem>> GetTodoItemsInList(int listId, int pageNumber = 1, int pageSize = 10);
    }
}