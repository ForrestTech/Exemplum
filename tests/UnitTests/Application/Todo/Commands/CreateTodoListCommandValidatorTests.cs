namespace Exemplum.UnitTests.Application.Todo.Commands;

using Exemplum.Application.TodoList.Commands;
using Exemplum.Domain.Todo;
using FluentValidation.TestHelper;

public class CreateTodoListCommandValidatorTests : TestBase
{
    [Fact]
    public void Validate_returns_the_created_item()
    {
        var fixture = CreateFixture();

        var command = fixture.Build<CreateTodoListCommand>()
            .WithAutoProperties()
            .With(x => x.Colour, Colour.Blue)
            .Create();
        
        var sut = new CreateTodoListCommandValidator();

        var result = sut.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_invalid_colour_should_have_errors()
    {
        var fixture = CreateFixture();

        var command = fixture.Build<CreateTodoListCommand>()
            .WithAutoProperties()
            .With(x => x.Colour, "Invalid")
            .Create();
        
        var sut = new CreateTodoListCommandValidator();

        var result = sut.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Colour);
    }
    
    [Fact]
    public void Validate_colour_should_be_optional()
    {
        var fixture = CreateFixture();

        var command = fixture.Build<CreateTodoListCommand>()
            .WithAutoProperties()
            .Create();

        command.Colour = null;
        
        var sut = new CreateTodoListCommandValidator();

        var result = sut.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Colour);
    }
}