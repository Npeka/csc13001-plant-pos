using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace csc13001_plant_pos.Views.Authentication;
public sealed partial class FormLayout : UserControl
{
    public FormLayout()
    {
        this.InitializeComponent();
    }

    public static readonly DependencyProperty ChildProperty =
           DependencyProperty.Register(
               nameof(Child),
               typeof(UIElement),
               typeof(FormLayout),
               new PropertyMetadata(null)
           );

    public UIElement Child
    {
        get => (UIElement)GetValue(ChildProperty);
        set => SetValue(ChildProperty, value);
    }
}
