using System.Text.Json;
using TodoMauiApp.Models;

namespace TodoMauiApp.Services;

public class TaskRepository : ITaskRepository
{
    private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");

    public async Task<List<TodoTask>> GetAllAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<TodoTask>();
        }

        await using var stream = File.OpenRead(_filePath);
        var tasks = await JsonSerializer.DeserializeAsync<List<TodoTask>>(stream);
        return tasks ?? new List<TodoTask>();
    }

    public async Task SaveAllAsync(List<TodoTask> tasks)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, tasks, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
