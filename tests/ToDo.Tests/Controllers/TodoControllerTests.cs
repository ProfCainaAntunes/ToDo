using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.Api.Controllers;
using ToDo.Api.DTOs;
using ToDo.Api.Models;
using ToDo.Api.Services;
using Xunit;

namespace ToDo.Tests.Controllers;

public class TodoControllerTests
{
    [Fact]
    public void GetAll_ShouldReturnOkWithTodoDtos()
    {
        // Arrange
        var service = new TodoService();
        service.Create("Task 1", Priority.Low);
        var controller = new TodoController(service);

        // Act
        var result = controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dtos = Assert.IsAssignableFrom<IEnumerable<TodoDto>>(okResult.Value);
        Assert.Single(dtos);
        Assert.Equal("Task 1", dtos.First().Title);
    }

    [Fact]
    public void Create_WithValidDto_ShouldReturnCreatedWithDto()
    {
        // Arrange
        var service = new TodoService();
        var controller = new TodoController(service);
        var dto = new CreateTodoDto { Title = "New Task", Priority = Priority.Medium };

        // Act
        var result = controller.Create(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var resultDto = Assert.IsType<TodoDto>(createdResult.Value);
        Assert.Equal("New Task", resultDto.Title);
        Assert.Equal(Priority.Medium, resultDto.Priority);
        Assert.False(resultDto.IsCompleted);
        Assert.Equal($"/tasks/{resultDto.Id}", createdResult.Location);
    }

    [Fact]
    public void Create_WithInvalidTitle_ShouldReturnBadRequest()
    {
        // Arrange
        var service = new TodoService();
        var controller = new TodoController(service);
        var dto = new CreateTodoDto { Title = "", Priority = Priority.Low };

        // Act
        var result = controller.Create(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public void Complete_WithValidId_ShouldReturnOkWithUpdatedDto()
    {
        // Arrange
        var service = new TodoService();
        var item = service.Create("Complete Me", Priority.High);
        var controller = new TodoController(service);

        // Act
        var result = controller.Complete(item.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultDto = Assert.IsType<TodoDto>(okResult.Value);
        Assert.True(resultDto.IsCompleted);
    }

    [Fact]
    public void Complete_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var service = new TodoService();
        var controller = new TodoController(service);
        var invalidId = Guid.NewGuid();

        // Act
        var result = controller.Complete(invalidId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void Delete_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var service = new TodoService();
        var item = service.Create("Delete Me", Priority.Low);
        var controller = new TodoController(service);

        // Act
        var result = controller.Delete(item.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Empty(service.GetAll());
    }

    [Fact]
    public void Delete_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var service = new TodoService();
        var controller = new TodoController(service);
        var invalidId = Guid.NewGuid();

        // Act
        var result = controller.Delete(invalidId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
