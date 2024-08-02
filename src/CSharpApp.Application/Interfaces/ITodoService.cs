namespace CSharpApp.Core.Interfaces;

public interface ITodoService
{
    Task<ReadOnlyCollection<TodoRecord>?> GetAllTodos();
    Task<TodoRecord?> GetTodoById(int id);
}