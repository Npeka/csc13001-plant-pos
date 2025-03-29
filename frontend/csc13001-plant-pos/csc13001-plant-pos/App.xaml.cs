using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using csc13001_plant_pos.Service;
using Microsoft.Extensions.DependencyInjection;
using csc13001_plant_pos.ViewModel.Authentication;

namespace csc13001_plant_pos;

public partial class App : Application
{
    public IHost Host
    {
        get;
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
            services.AddSingleton<IAuthenticationService, AuthenticationService>();

            // Views and ViewModels
            services.AddTransient<LoginViewModel>();

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
