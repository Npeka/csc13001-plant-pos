using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.Views.Authentication;
public sealed partial class FormTitle : UserControl
{
    public FormTitle()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }

    public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(FormTitle),
                new PropertyMetadata(string.Empty)
            );

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}
