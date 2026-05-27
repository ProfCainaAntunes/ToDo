using System;
using System.Linq;
using ToDo.Api.Models;
using ToDo.Api.Services;
using Xunit;

namespace ToDo.Tests.Services;

public class TodoServiceTests
{
    [Fact]
    public void Create_WithValidTitle_ShouldReturnTodoItem()
    {
        // Arrange
        var service = new TodoService();
        string expectedTitle = "Test Task";
        Priority expectedPriority = Priority.High;

        // Act
        var result = service.Create(expectedTitle, expectedPriority);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(expectedTitle, result.Title);
        Assert.False(result.IsCompleted);
        Assert.Equal(expectedPriority, result.Priority);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidTitle_ShouldThrowArgumentException(string? invalidTitle)
    {
        // Arrange
        var service = new TodoService();

        // Act & Assert
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = Assert.Throws<ArgumentException>(() => service.Create(invalidTitle, Priority.Medium));
#pragma warning restore CS8604
        Assert.Equal("Title is required", exception.Message);
    }

    [Fact]
    public void GetAll_ShouldReturnAllCreatedItems()
    {
        // Arrange
        var service = new TodoService();
        service.Create("Task 1", Priority.Low);
        service.Create("Task 2", Priority.Medium);

        // Act
        var result = service.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Title == "Task 1");
        Assert.Contains(result, t => t.Title == "Task 2");
    }

    [Fact]
    public void Complete_WithValidId_ShouldMarkAsCompleted()
    {
        // Arrange
        var service = new TodoService();
        var item = service.Create("Test Task", Priority.Low);

        // Act
        var result = service.Complete(item.Id);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompleted);

        // Verify state remains updated in GetAll
        var updatedItem = service.GetAll().First(t => t.Id == item.Id);
        Assert.True(updatedItem.IsCompleted);
    }

    [Fact]
    public void Complete_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var service = new TodoService();
        var invalidId = Guid.NewGuid();

        // Act
        var result = service.Complete(invalidId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_WithValidId_ShouldRemoveItemAndReturnTrue()
    {
        // Arrange
        var service = new TodoService();
        var item = service.Create("Test Task", Priority.Medium);

        // Act
        var deleteResult = service.Delete(item.Id);
        var allItems = service.GetAll();

        // Assert
        Assert.True(deleteResult);
        Assert.Empty(allItems);
    }

    [Fact]
    public void Delete_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var service = new TodoService();
        var invalidId = Guid.NewGuid();

        // Act
        var deleteResult = service.Delete(invalidId);

        // Assert
        Assert.False(deleteResult);
    }
}
