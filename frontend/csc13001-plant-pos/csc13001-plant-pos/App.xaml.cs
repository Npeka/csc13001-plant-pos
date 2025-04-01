using System;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using csc13001_plant_pos.Service;
using csc13001_plant_pos.ViewModel.Authentication;
using csc13001_plant_pos.ViewModel;
using Syncfusion.Licensing;

namespace csc13001_plant_pos;

public partial class App : Application
{
    public IHost Host { get; }

    public IConfiguration Configuration { get; }

    public static T GetService<T>() where T : class
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
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        Configuration = builder.Build();

        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((context, services) =>
            {
                var licenseKey = Configuration["Syncfusion:LicenseKey"];
                if (!string.IsNullOrEmpty(licenseKey))
                {
                    SyncfusionLicenseProvider.RegisterLicense(licenseKey);
                }

                var baseAddress = Configuration["ApiSettings:BaseAddress"] ?? "http://localhost:8080/api/";
                var httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };

                // Services
                services.AddSingleton(httpClient);
                services.AddSingleton<UserSessionService>();
                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<ICategoryService, CategoryService>();
                services.AddSingleton<ICustomerService, CustomerService>();
                services.AddSingleton<IDiscountProgramService, DiscountProgramService>();
                services.AddSingleton<IInventoryService, InventoryService>();
                services.AddSingleton<IOrderService, OrderService>();
                services.AddSingleton<IProductService, ProductService>();
                services.AddSingleton<IStaffService, StaffService>();
               

                // Views and ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddTransient<ForgotPasswordViewModel>();
                services.AddTransient<ResetPasswordViewModel>();
                services.AddTransient<AuthenticationViewModel>();
                services.AddTransient<StaffProfileViewModel>();
                services.AddTransient<SaleViewModel>();
                services.AddTransient<BillViewModel>();
                services.AddTransient<StaffProfileViewModel>();
                services.AddSingleton<AddCustomerViewModel>();
                services.AddSingleton<CustomerProfileViewModel>();
                services.AddSingleton<OrderViewModel>();
                services.AddSingleton<WarehouseManagementViewModel>();
                services.AddSingleton<DiscountManagementViewModel>();
                services.AddSingleton<StatisticViewModel>();
                services.AddSingleton<StaffManagementViewModel>();
                services.AddSingleton<CustomerManagementViewModel>();
            })
            .Build();
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
