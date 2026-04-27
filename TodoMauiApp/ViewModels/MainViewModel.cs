using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoMauiApp.Models;
using TodoMauiApp.Services;

namespace TodoMauiApp.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ITaskRepository _taskRepository;
    private readonly List<TodoTask> _allTasks = new();

    private string _newTitle = string.Empty;
    private string _newDescription = string.Empty;
    private string _filter = "Toutes";

    public MainViewModel(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;

        Tasks = new ObservableCollection<TodoTask>();

        AddTaskCommand = new Command(async () => await AddTaskAsync(), CanAddTask);
        ToggleTaskCommand = new Command<TodoTask>(async task => await ToggleTaskAsync(task));
        DeleteTaskCommand = new Command<TodoTask>(async task => await DeleteTaskAsync(task));
        RefreshCommand = new Command(async () => await LoadTasksAsync());
        ApplyFilterCommand = new Command<string>(ApplyFilter);

        Task.Run(LoadTasksAsync);
    }

    public ObservableCollection<TodoTask> Tasks { get; }

    public ICommand AddTaskCommand { get; }
    public ICommand ToggleTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ApplyFilterCommand { get; }

    public string NewTitle
    {
        get => _newTitle;
        set
        {
            if (SetProperty(ref _newTitle, value))
            {
                ((Command)AddTaskCommand).ChangeCanExecute();
            }
        }
    }

    public string NewDescription
    {
        get => _newDescription;
        set => SetProperty(ref _newDescription, value);
    }

    public string Filter
    {
        get => _filter;
        set => SetProperty(ref _filter, value);
    }

    public int RemainingCount => _allTasks.Count(task => !task.IsCompleted);

    private async Task LoadTasksAsync()
    {
        var items = await _taskRepository.GetAllAsync();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            _allTasks.Clear();
            _allTasks.AddRange(items.OrderByDescending(t => t.CreatedAt));
            UpdateVisibleTasks();
        });
    }

    private bool CanAddTask()
    {
        return !string.IsNullOrWhiteSpace(NewTitle);
    }

    private async Task AddTaskAsync()
    {
        var task = new TodoTask
        {
            Title = NewTitle.Trim(),
            Description = NewDescription.Trim()
        };

        _allTasks.Insert(0, task);
        UpdateVisibleTasks();
        await PersistAsync();

        NewTitle = string.Empty;
        NewDescription = string.Empty;
    }

    private async Task ToggleTaskAsync(TodoTask? task)
    {
        if (task is null)
        {
            return;
        }

        task.IsCompleted = !task.IsCompleted;
        UpdateVisibleTasks();
        await PersistAsync();
    }

    private async Task DeleteTaskAsync(TodoTask? task)
    {
        if (task is null)
        {
            return;
        }

        _allTasks.RemoveAll(t => t.Id == task.Id);
        UpdateVisibleTasks();
        await PersistAsync();
    }

    private async Task PersistAsync()
    {
        await _taskRepository.SaveAllAsync(_allTasks);
    }

    private void ApplyFilter(string? filter)
    {
        Filter = string.IsNullOrWhiteSpace(filter) ? "Toutes" : filter;
        UpdateVisibleTasks();
    }

    private void UpdateVisibleTasks()
    {
        IEnumerable<TodoTask> filtered = _allTasks;

        if (Filter == "À faire")
        {
            filtered = _allTasks.Where(task => !task.IsCompleted);
        }
        else if (Filter == "Terminées")
        {
            filtered = _allTasks.Where(task => task.IsCompleted);
        }

        Tasks.Clear();
        foreach (var task in filtered.OrderByDescending(t => t.CreatedAt))
        {
            Tasks.Add(task);
        }

        OnPropertyChanged(nameof(RemainingCount));
    }
}
