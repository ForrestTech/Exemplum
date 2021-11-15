namespace Exemplum.UnitTests.Application.Todo.Queries;

using Exemplum.Application.TodoList.Queries;
using Exemplum.Domain.Todo;

public class GetTodoListsQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_returns_lists_in_database()
    {
        var fixture = CreateFixture();

        fixture.SeedData((context) =>
        {
            context.TodoLists.Add(new TodoList("Shopping", Colour.Blue));
        });

        var query = new GetTodoListsQuery();
        var sut = fixture.Create<GetTodoListsQueryHandler>();

        var result = await sut.Handle(query, CancellationToken.None);

        result.Items?.Count.Should().Be(1);
    }
}