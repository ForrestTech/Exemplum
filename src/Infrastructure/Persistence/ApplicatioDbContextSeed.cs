namespace Exemplum.Infrastructure.Persistence;

using Domain.Todo;
using Microsoft.EntityFrameworkCore;

public static class ApplicationDbContextSeed
{
    public static void SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        var lists = context.TodoLists
            .IgnoreQueryFilters()
            .ToList();

        if (!lists.Any())
        {
            var list = new TodoList("Shopping", Colour.Blue);
            list.AddToDo(new List<TodoItem>
            {
                new("Apples") {Done = true},
                new("Milk") {Done = true},
                new("Bread") {Done = true},
                new("Toilet paper"),
                new("Pasta"),
                new("Tissues"),
                new("Tuna"),
                new("Water")
            });

            context.TodoLists.Add(list);

            context.SaveChanges();
        }
    }
}