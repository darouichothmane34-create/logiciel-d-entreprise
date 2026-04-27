using TodoMauiApp.Models;

namespace TodoMauiApp.Services;

public interface ITaskRepository
{
    Task<List<TodoTask>> GetAllAsync();
    Task SaveAllAsync(List<TodoTask> tasks);
}
