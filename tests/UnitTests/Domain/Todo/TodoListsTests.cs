namespace Exemplum.UnitTests.Domain.Todo
{
    using Exemplum.Domain.Todo;
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class TodoListsTests
    {
        [Fact]
        public void AddToDo_should_update_item_list()
        {
            var sut = new TodoList("List", Colour.Blue);

            const string todoTitle = "New todo";
            sut.AddToDo(new TodoItem(todoTitle));

            sut.Items.Count().Should().Be(1);
            sut.Items.First().Title.Should().Be(todoTitle);
        }
        
        [Fact]
        public void AddToDo_should_raise_domain_event()
        {
            var sut = new TodoList("List", Colour.Blue);

            sut.AddToDo(new TodoItem("New todo"));

            sut.DomainEvents.Count.Should().Be(1);
        }

        [Fact]
        public void AddToDo_list_should_raise_domain_event()
        {
            var sut = new TodoList("List", Colour.Blue);

            sut.AddToDo(new List<TodoItem> { new TodoItem("New todo"), new TodoItem("Another todo") });

            sut.Items.Count().Should().Be(2);
            sut.DomainEvents.Count.Should().Be(2);
        }
    }
}