using System;
using ToDo.Api.Models;

namespace ToDo.Api.DTOs;

public class TodoDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public Priority Priority { get; set; }
}
