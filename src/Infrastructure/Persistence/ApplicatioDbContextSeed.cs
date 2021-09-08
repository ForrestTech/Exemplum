namespace Infrastructure.Persistence
{
    using Domain.Todo;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            var lists = await context.TodoLists.ToListAsync();
            if (!lists.Any())
            {
                var list = new TodoList("Shopping", Colour.Blue);
                list.AddToDo(new List<TodoItem>
                {
                    new TodoItem("Apples") { Done = true },
                    new TodoItem("Milk") { Done = true },
                    new TodoItem("Bread") { Done = true },
                    new TodoItem("Toilet paper"),
                    new TodoItem("Pasta"),
                    new TodoItem("Tissues"),
                    new TodoItem("Tuna"),
                    new TodoItem("Water")
                });

                context.TodoLists.Add(list);

                await context.SaveChangesAsync();
            }
        }
    }
}