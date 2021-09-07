namespace Domain.UnitTests.Todo
{
    using Domain.Todo;
    using Shouldly;
    using System.Diagnostics.SymbolStore;
    using Xunit;

    public class TodoItemsTests
    {
        [Fact]
        public void MarkAsDone_should_complete_item()
        {
            var sut = new TodoItem("New todo");
            
            sut.MarkAsDone();
            
            sut.Done.ShouldBe(true);
        }
    }
}
