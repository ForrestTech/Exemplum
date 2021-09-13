namespace Application.UnitTests.Todo
{
    using Application.Todo.Queries;
    using Domain.Todo;
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Linq;
    using TodoList.Queries;
    using Xunit;

    public class GetCompletedTodoItemsQueryTests
    {
        [Fact]
        public void ApplyQuery_only_returns_completed_items()
        {
            var todos = new List<TodoItem>
            {
                new TodoItem("Apples") { Done = true },
                new TodoItem("Milk") { Done = true },
                new TodoItem("Bread") { Done = true },
                new TodoItem("Toilet paper"),
                new TodoItem("Pasta"),
                new TodoItem("Tissues"),
                new TodoItem("Tuna"),
                new TodoItem("Water")
            };

            var sut = new GetCompletedTodoItemsQuery();

            var actual = sut.ApplyQuery(todos.AsQueryable())
                .ToList();

            actual.Count.Should().Be(3);
        }
    }
}