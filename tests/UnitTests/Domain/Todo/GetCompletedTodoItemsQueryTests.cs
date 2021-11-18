namespace Exemplum.UnitTests.Domain.Todo;

    using Exemplum.Application.Todo.Queries;
    using Exemplum.Domain.Todo;

    public class GetCompletedTodoItemsQueryTests
    {
        [Fact]
        public void ApplyQuery_only_returns_completed_items()
        {
            var todos = new List<TodoItem>
            {
                new("Apples") { Done = true },
                new("Milk") { Done = true },
                new("Bread") { Done = true },
                new("Toilet paper"),
                new("Pasta"),
                new("Tissues"),
                new("Tuna"),
                new("Water")
            };

            var sut = new GetCompletedTodoItemsQuery();

            var actual = sut.ApplyQuery(todos.AsQueryable())
                .ToList();

            actual.Count.Should().Be(3);
        }
    }
