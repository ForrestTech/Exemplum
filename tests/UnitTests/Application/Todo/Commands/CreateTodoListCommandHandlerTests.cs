namespace Exemplum.UnitTests.Application.Todo.Commands;

using Exemplum.Application.TodoList.Commands;
using Exemplum.Domain.Todo;

public class CreateTodoListCommandHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_returns_the_created_item()
    {
        var fixture = CreateFixture();
        fixture.InjectDefaultValidator<CreateTodoListCommand>();

        var command = fixture.Build<CreateTodoListCommand>()
            .With(x => x.Colour, Colour.Blue)
            .Create();

        var sut = fixture.Create<CreateTodoListCommandHandler>();

        var result = await sut.Handle(command, CancellationToken.None);

        result.AsT0.Title.Should().Be(command.Title);
    }
    
    [Fact]
    public async Task Handle_returns_validation_failed_if_validator_fails()
    {
        var fixture = CreateFixture();
        fixture.InjectFailingValidator<CreateTodoListCommand>();

        var command = fixture.Build<CreateTodoListCommand>()
            .With(x => x.Colour, Colour.Blue)
            .Create();

        var sut = fixture.Create<CreateTodoListCommandHandler>();

        var result = await sut.Handle(command, CancellationToken.None);

        result.Switch(
            _ => Assert.Fail(),
            failed => failed.Errors.Should().NotBeEmpty());
    }

    [Fact]
    public async Task Handle_with_invalid_colour_throw()
    {
        var fixture = CreateFixture();
        fixture.InjectDefaultValidator<CreateTodoListCommand>();

        var command = fixture.Build<CreateTodoListCommand>()
            .With(x => x.Colour, "Invalid")
            .Create();

        var sut = fixture.Create<CreateTodoListCommandHandler>();

        Func<Task> act = async () => await sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnsupportedColourException>();
    }
}

public class TestStub
{
    public string Name { get; set; }

    public PriorityLevel PriorityLevel { get; set; }
}