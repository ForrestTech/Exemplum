namespace Exemplum.WebApp.Features.TodoLists
{
    public class TodoItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public bool Done { get; set; }
    }
}