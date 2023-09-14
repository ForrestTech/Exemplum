namespace Exemplum.IntegrationTests;

public static class Api
{
    public const string List = "api/todolist";
    public static string Items(int listId) => $"{List}/{listId}/todo";
    
    public static string Completed(int listId) => $"{List}/{listId}/todo/completed";
}