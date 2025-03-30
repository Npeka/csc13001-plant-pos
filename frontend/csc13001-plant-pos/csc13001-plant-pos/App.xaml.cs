using System;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using csc13001_plant_pos.Service;
using Microsoft.Extensions.DependencyInjection;
using csc13001_plant_pos.ViewModel.Authentication;
using csc13001_plant_pos.ViewModel;

namespace csc13001_plant_pos;

public partial class App : Application
{
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


    public App()
    {
        this.InitializeComponent();
        this.RequestedTheme = ApplicationTheme.Light;

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            var baseAddress = context.Configuration.GetSection("ApiSettings:BaseAddress").Value;
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress ?? "http://localhost:8080/api/")
            };
            services.AddSingleton(httpClient);
            services.AddSingleton<UserSessionService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();

            // Views and ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<ForgotPasswordViewModel>();
            services.AddTransient<ResetPasswordViewModel>();
            services.AddTransient<AuthenticationViewModel>();
            services.AddTransient<StaffProfileViewModel>();

        }).
        Build();
    }
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        m_window.Activate();
    }
    public MainWindow? GetMainWindow()
    {
        return m_window as MainWindow;
    }

    private Window? m_window;
}
