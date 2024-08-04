using CSharpApp.Application.Dtos;

namespace CSharpApp.Application.Interfaces;

public interface ITodoService
{
    Task<ReadOnlyCollection<TodoRecord>?> GetAllTodosAsync();
    Task<TodoRecord?> GetTodoByIdAsync(int id);
}