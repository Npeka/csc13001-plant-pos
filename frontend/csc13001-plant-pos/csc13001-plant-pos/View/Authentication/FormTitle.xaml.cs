using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace csc13001_plant_pos.View.Authentication
{
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
}
