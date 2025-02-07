using System.Diagnostics;

using csc13001_plant_pos.Activation;
using csc13001_plant_pos.Contracts.Services;
using csc13001_plant_pos.Core.Contracts.Services;
using csc13001_plant_pos.Core.Services;
using csc13001_plant_pos.Data.Contexts;
using csc13001_plant_pos.Helpers;
using csc13001_plant_pos.Models;
using csc13001_plant_pos.Notifications;
using csc13001_plant_pos.Services;
using csc13001_plant_pos.ViewModels;
using csc13001_plant_pos.ViewModels.Authentication;
using csc13001_plant_pos.Views;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Windows.UI;

namespace csc13001_plant_pos;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar
    {
        get; set;
    }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            services.Configure<SmtpSettings>(context.Configuration.GetSection("SmtpSettings"));

            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IOrderService, OrderService>();

            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<AuthenticationViewModel>();
            services.AddTransient<AuthenticationPage>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<ForgotPasswordViewModel>();
            services.AddTransient<ResetPasswordViewModel>();

            services.AddTransient<OrderViewModel>();
            services.AddTransient<OrderPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<DataGridViewModel>();
            services.AddTransient<DataGridPage>();
            services.AddTransient<ContentGridDetailViewModel>();
            services.AddTransient<ContentGridDetailPage>();
            services.AddTransient<ContentGridViewModel>();
            services.AddTransient<ContentGridPage>();
            services.AddTransient<ListDetailsViewModel>();
            services.AddTransient<ListDetailsPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<ShellPage>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));

            // Database MySQL
            var mySqlConnection = context.Configuration.GetConnectionString("MySqlConnection");
            services.AddDbContext<ApplicationDbContext>((
                options => options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection))));

            // Redis
            services.AddSingleton<RedisService>(provider =>
            {
                var redisConnection = context.Configuration.GetConnectionString("RedisConnection");
                if (string.IsNullOrEmpty(redisConnection))
                {
                    redisConnection = "localhost:6379";
                }
                return new RedisService(redisConnection);
            });
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
