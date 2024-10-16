using CheetahExam.Domain.Common;
using CheetahExam.Domain.Entities;

namespace CheetahExam.Domain.Events;

public class TodoItemCompletedEvent : BaseEvent
{
    public TodoItemCompletedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}
