namespace Exemplum.Infrastructure.Persistence;

using Domain.Todo;
using Microsoft.EntityFrameworkCore;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        var lists = await context.TodoLists
            .IgnoreQueryFilters()
            .ToListAsync();

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

            await context.SaveChangesAsync();
        }
    }
}