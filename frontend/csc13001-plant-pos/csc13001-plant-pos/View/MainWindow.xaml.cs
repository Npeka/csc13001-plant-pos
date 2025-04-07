using csc13001_plant_pos.View;
using Microsoft.UI.Xaml;

namespace csc13001_plant_pos
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            mainFrame.IsNavigationStackEnabled = true;
        }
        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (mainFrame.Content == null)
            {
                mainFrame.Navigate(typeof(AuthenticationPage));
            }
        }
    }
}
