namespace Application.UnitTests
{
    using Domain.Todo;
    using Shouldly;
    using System.Collections.Generic;
    using System.Linq;
    using Todo.Queries;
    using Xunit;

    public class GetCompletedTodoItemsQueryTests
    {
        [Fact]
        public void ApplyQuery_only_returns_completed_items()
        {
            var todos = new List<TodoItem>{
                new TodoItem { Title = "Apples", Done = true },
                new TodoItem { Title = "Milk", Done = true },
                new TodoItem { Title = "Bread", Done = true },
                new TodoItem { Title = "Toilet paper" },
                new TodoItem { Title = "Pasta" },
                new TodoItem { Title = "Tissues" },
                new TodoItem { Title = "Tuna" },
                new TodoItem { Title = "Water" }
            };
            
            var sut = new GetCompletedTodoItemsQuery();
            
            var actual = sut.ApplyQuery(todos.AsQueryable())
                .ToList();

            actual.Count.ShouldBe(3);
        }
    }
}