using System.ComponentModel.DataAnnotations;
using ToDo.Api.Models;

namespace ToDo.Api.DTOs;

public class CreateTodoDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [MinLength(1, ErrorMessage = "O título não pode estar vazio.")]
    public string Title { get; set; } = string.Empty;

    public Priority Priority { get; set; }
}
