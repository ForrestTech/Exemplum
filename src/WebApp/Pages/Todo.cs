namespace WebApp.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    public partial class Todo
    {
        public readonly List<TodoItem> Todos = new();
        public string NewTodo = string.Empty;

        private int CountOfTodoItems
        {
            get { return Todos.Count(x => !x.IsDone); }
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