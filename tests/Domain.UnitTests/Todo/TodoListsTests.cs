namespace Domain.UnitTests.Todo
{
    using Domain.Todo;
    using FluentAssertions;
    using Xunit;

    public class TodoListsTests
    {
        [Fact]
        public void AddToDo_should_raise_domain_event()
        {
            var sut = new TodoList("List", Colour.Red);
            
            sut.AddToDo(new TodoItem("New todo"));
            
            sut.DomainEvents.Count.Should().Be(1);
        }
    }
}