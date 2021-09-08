namespace Domain.UnitTests.Todo
{
    using Domain.Todo;
    using FluentAssertions;
    using Xunit;

    public class TodoListsTests
    {
        [Fact]
        public void Foo()
        {
            var sut = new TodoList();
            
            sut.AddToDo(new TodoItem("New todo"));
            
            sut.DomainEvents.Count.Should().Be(1);
        }
    }
}