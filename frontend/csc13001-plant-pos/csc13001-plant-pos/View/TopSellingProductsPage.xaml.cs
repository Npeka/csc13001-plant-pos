using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace csc13001_plant_pos.View
{
    public sealed partial class TopSellingProductsPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Products> AllProducts = new ObservableCollection<Products>();
        private ObservableCollection<Products> FilteredProducts = new ObservableCollection<Products>();
        private const int ItemsPerPage = 10;
        private int currentPage = 1;
        private int totalPages = 10;

        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                UpdatePagination();
            }
        }

        public int TotalPages
        {
            get => totalPages;
            set
            {
                totalPages = value;
            }
        }

        public TopSellingProductsPage()
        {
            this.InitializeComponent();
            LoadData();
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void LoadData()
        {
            for (int i = 1; i <= 100; i++)
            {
                AllProducts.Add(new Products
                {
                    ID = i,
                    Name = $"Sản phẩm {i}",
                    SoldQuantity = i * 2,
                    StockQuantity = 100 - i,
                    Revenue = i * 1000,
                    ImageUrl = "https://via.placeholder.com/50"
                });
            }

            totalPages = (AllProducts.Count + ItemsPerPage - 1) / ItemsPerPage;
            CurrentPage = 1;
            UpdatePagination();
        }

        private void UpdatePagination()
        {
            FilteredProducts.Clear();
            var items = AllProducts.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);
            foreach (var item in items)
            {
                FilteredProducts.Add(item);
            }
            EmployeesGrid.ItemsSource = FilteredProducts;
            EmployeeCount.Text = $"{AllProducts.Count} sản phẩm";
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
        }
        private void PositionFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // do nothing 
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // do nothing
        }
    }

    public class Products
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SoldQuantity { get; set; }
        public int StockQuantity { get; set; }
        public double Revenue { get; set; }
        public string ImageUrl { get; set; }
    }

}
