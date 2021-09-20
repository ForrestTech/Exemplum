namespace Exemplum.WebApp.Features.TodoLists
{
    using Client;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class TodoPage
    {
        public List<TodoItem> Todos = new();
        public string NewTodo = string.Empty;

        [Inject] 
        public ITodoClient Client { get; set; } = default!;
        
        [Inject] 
        public ILogger<TodoPage> Logger { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("Getting todo list items");
            
            var todoPage = await Client.GetTodoItemsInList(1);

            Todos = todoPage.Items;
        }

        private int CountOfTodoItems
        {
            get { return Todos.Count(x => !x.Done); }
        }

        private void AddTodo()
        {
            if (!string.IsNullOrWhiteSpace(NewTodo))
            {
                Todos.Add(new TodoItem { Title = NewTodo });
                NewTodo = string.Empty;
            }
        }
    }
}