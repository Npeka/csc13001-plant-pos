using CommunityToolkit.Mvvm.ComponentModel;

namespace csc13001_plant_pos.Model
{
    public class StockReceiptItem : ObservableObject
    {
        public Product Product { get; set; }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        private decimal _purchasePrice;
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set => SetProperty(ref _purchasePrice, value);
        }
    }
}