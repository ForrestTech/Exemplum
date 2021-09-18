namespace Exemplum.WebApp.Features.TodoLists
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class TodoListDetailsPage
    {
        List<TodoItem> todoItems = new List<TodoItem>();
        TodoList list = new TodoList();

        [Parameter]
        public int ListId { get; set; }
        
        [Inject]
        public ITodoClient Client { get; set; } = default!;

        [Inject]
        public ILogger<TodoListDetailsPage> Logger { get; set; } = default!;

        [Inject]
        public NavigationManager NavManager { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("Getting todo list");

            list = await Client.GetTodoList(ListId);

            var pagedList = await Client.GetTodoItemsInList(ListId, 1, 100);
            todoItems = pagedList.Items;
        }
    }
}