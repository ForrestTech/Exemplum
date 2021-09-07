namespace Infrastructure.Persistence
{
    using Domain.Todo;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.TodoLists.Any())
            {
                context.TodoLists.Add(new TodoList(new List<TodoItem>
                {
                    new TodoItem { Title = "Apples", Done = true },
                    new TodoItem { Title = "Milk", Done = true },
                    new TodoItem { Title = "Bread", Done = true },
                    new TodoItem { Title = "Toilet paper" },
                    new TodoItem { Title = "Pasta" },
                    new TodoItem { Title = "Tissues" },
                    new TodoItem { Title = "Tuna" },
                    new TodoItem { Title = "Water" }
                }) { Title = "Shopping", Colour = Colour.Blue });

                await context.SaveChangesAsync();
            }
        }
    }
}