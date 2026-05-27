using System;
using System.Collections.Generic;
using ToDo.Api.Models;

namespace ToDo.Api.Services;

public interface ITodoService
{
    IEnumerable<TodoItem> GetAll();

    TodoItem Create(string title, Priority priority);

    TodoItem? Complete(Guid id);

    bool Delete(Guid id);
}
