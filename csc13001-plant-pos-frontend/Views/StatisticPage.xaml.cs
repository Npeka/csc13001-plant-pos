using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace csc13001_plant_pos_frontend.Views;

public sealed partial class StatisticPage : Page, INotifyPropertyChanged
{
    private ObservableCollection<SalesData> _salesDataList;
    public ObservableCollection<SalesData> SalesDataList
    {
        get => _salesDataList;
        set
        {
            _salesDataList = value;
            OnPropertyChanged(nameof(SalesDataList));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public StatisticPage()
    {
        this.InitializeComponent();
        LoadData();
        this.DataContext = this;
    }

    private void LoadData()
    {
        SalesDataList = new ObservableCollection<SalesData>
        {
            new SalesData { Name = "Gross Sales", Sales = "$10,904.00", Refunds = "$0.00", Net = "$10,904.00" },
            new SalesData { Name = "Net Sales", Sales = "$10,904.00", Refunds = "$0.00", Net = "$10,904.00" },
            new SalesData { Name = "Total Collected", Sales = "$10,904.00", Refunds = "$0.00", Net = "$10,904.00" }
        };
    }
}

public class SalesData
{
    public string Name
    {
        get; set;
    }
    public string Sales
    {
        get; set;
    }
    public string Refunds
    {
        get; set;
    }
    public string Net
    {
        get; set;
    }
}
