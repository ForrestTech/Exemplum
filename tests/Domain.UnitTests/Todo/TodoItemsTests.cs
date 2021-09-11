namespace Domain.UnitTests.Todo
{
    using Domain.Todo;
    using FluentAssertions;
    using Infrastructure.DateAndTime;
    using System;
    using System.Diagnostics.SymbolStore;
    using Xunit;

    public class TodoItemsTests
    {
        [Fact]
        public void NewTodo_should_have_default_priority_and_no_reminder()
        {
            var sut = new TodoItem("New todo");

            sut.Done.Should().Be(false);
            sut.Priority.Should().Be(PriorityLevel.None);
            sut.Reminder.Should().Be(null);
        }
        
        [Fact]
        public void MarkAsDone_should_complete_item()
        {
            var sut = new TodoItem("New todo");
            
            sut.MarkAsDone();
            
            sut.Done.Should().Be(true);
        }
        
        [Fact]
        public void SetPriority_should_change_priority_level()
        {
            var priorityLevel = PriorityLevel.High;
            
            var sut = new TodoItem("New todo");
            sut.SetPriority(priorityLevel, new Clock());
            
            sut.Priority.Should().Be(priorityLevel);
        }
        
        [Fact]
        public void SetPriority_should_set_reminder_time()
        {
            var priorityLevel = PriorityLevel.High;
            
            var sut = new TodoItem("New todo");
            sut.SetPriority(priorityLevel, new Clock());

            sut.Reminder.Should().NotBe(null);
        }
        
        [Fact]
        public void SetPriority_should_set_reminder_to_priority_time()
        {
            var priorityLevel = PriorityLevel.High;
            var instance = DateTime.UtcNow;
            var reminderTime = instance.Add(priorityLevel.ReminderTime);
            
            var sut = new TodoItem("New todo");
            sut.SetPriority(priorityLevel, new Clock(instance));

            sut.Reminder.Should().Be(reminderTime);
        }
    }
}
