﻿namespace Exemplum.Domain.Todo.Events;

using Common;
    using System;

public class TodoItemCreatedEvent : DomainEvent
{
    public TodoItemCreatedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}