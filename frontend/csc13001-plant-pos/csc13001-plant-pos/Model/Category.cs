using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace csc13001_plant_pos.Model
{
    public class Category : ObservableObject
    {
        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
