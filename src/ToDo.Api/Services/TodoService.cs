using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.Api.Models;

namespace ToDo.Api.Services;

public class TodoService : ITodoService
{
    private readonly List<TodoItem> _todos = new();
    private readonly object _lock = new();

    public IEnumerable<TodoItem> GetAll()
    {
        lock (_lock)
        {
            return _todos.ToList();
        }
    }

    public TodoItem Create(string title, Priority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required");
        }

        var newItem = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            IsCompleted = false,
            Priority = priority
        };

        lock (_lock)
        {
            _todos.Add(newItem);
        }

        return newItem;
    }

    public TodoItem? Complete(Guid id)
    {
        lock (_lock)
        {
            var item = _todos.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return null;
            }

            item.IsCompleted = true;
            return item;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var item = _todos.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return false;
            }

            _todos.Remove(item);
            return true;
        }
    }
}
