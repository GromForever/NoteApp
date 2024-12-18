using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using NoteApp.Services;
using NoteApp.ViewModel;

namespace NoteApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ServiceProvider serviceProvider;

    public App()
    {
        var services = new ServiceCollection();

        // Регистрация сервисов
        services.AddSingleton<IProjectService, JsonProjectService>();
        services.AddTransient<MainWindowViewModel>();

        serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    }
}