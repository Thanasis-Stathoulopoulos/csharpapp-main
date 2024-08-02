namespace CSharpApp.Core.Interfaces;

public interface ITodoService
{
    Task<ReadOnlyCollection<TodoRecord>?> GetAllTodosAsync();
    Task<TodoRecord?> GetTodoByIdAsync(int id);
}