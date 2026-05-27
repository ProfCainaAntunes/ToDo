using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.Api.DTOs;
using ToDo.Api.Models;
using ToDo.Api.Services;

namespace ToDo.Api.Controllers;

[ApiController]
[Route("tasks")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TodoDto>> GetAll()
    {
        var items = _todoService.GetAll();
        var dtos = items.Select(item => new TodoDto
        {
            Id = item.Id,
            Title = item.Title,
            IsCompleted = item.IsCompleted,
            Priority = item.Priority
        });

        return Ok(dtos);
    }

    [HttpPost]
    public ActionResult<TodoDto> Create([FromBody] CreateTodoDto dto)
    {
        try
        {
            var createdItem = _todoService.Create(dto.Title, dto.Priority);
            var resultDto = new TodoDto
            {
                Id = createdItem.Id,
                Title = createdItem.Title,
                IsCompleted = createdItem.IsCompleted,
                Priority = createdItem.Priority
            };

            //return NoContent();
            return Created($"/tasks/{resultDto.Id}", resultDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("{id:guid}/complete")]
    public ActionResult<TodoDto> Complete(Guid id)
    {
        var completedItem = _todoService.Complete(id);
        if (completedItem == null)
        {
            return NotFound(new { Message = $"Task with ID {id} not found." });
        }

        var resultDto = new TodoDto
        {
            Id = completedItem.Id,
            Title = completedItem.Title,
            IsCompleted = completedItem.IsCompleted,
            Priority = completedItem.Priority
        };

        //return NoContent();
        return Ok(resultDto);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _todoService.Delete(id);
        if (!deleted)
        {
            return NotFound(new { Message = $"Task with ID {id} not found." });
        }

        return NoContent();
    }
}
