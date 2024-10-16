using CheetahExam.Domain.Common;
using CheetahExam.Domain.Entities;

namespace CheetahExam.Domain.Events;

public class TodoItemCreatedEvent : BaseEvent
{
    public TodoItemCreatedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}
