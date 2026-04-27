using Microsoft.Extensions.DependencyInjection;
using TodoMauiApp.Services;
using TodoMauiApp.ViewModels;

namespace TodoMauiApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        BindingContext = Application.Current?.Handler?.MauiContext?.Services.GetService<MainViewModel>()
                         ?? new MainViewModel(new TaskRepository());
    }
}
