namespace Exemplum.WebApp.Features.TodoLists
{
    public class TodoList
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        
        public string Colour { get; set; } = string.Empty;

        public bool InEditMode { get; set; }
    }
}