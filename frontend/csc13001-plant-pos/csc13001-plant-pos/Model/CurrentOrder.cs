using CommunityToolkit.Mvvm.ComponentModel;

namespace csc13001_plant_pos.Model
{
    public class CurrentOrder : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private string _note;
        public string Note {
            get => _note;
            set => SetProperty(ref _note, value);
        }
        public string ImageUrl { get; set; }
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }
        public decimal Price { get; set; }
    }
}
