namespace Domain.UnitTests.Todo
{
    using Domain.Todo;
    using Shouldly;
    using Xunit;

    public class TodoListsTests
    {
        [Fact]
        public void Foo()
        {
            var sut = new TodoList();
            
            sut.AddToDo(new TodoItem("New todo"));
            
            sut.DomainEvents.Count.ShouldBe(1);
        }
    }
}