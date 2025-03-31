using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using csc13001_plant_pos.View.Authentication;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace csc13001_plant_pos.View.UI
{
    public sealed partial class PageTitle : UserControl
    {
        public PageTitle()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty TitleProperty =
                DependencyProperty.Register(
                    nameof(Title),
                    typeof(string),
                    typeof(PageTitle),
                    new PropertyMetadata(string.Empty)
                );

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}
