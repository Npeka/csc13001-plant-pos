using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using csc13001_plant_pos.Model;
using csc13001_plant_pos.ViewModel;

namespace csc13001_plant_pos.View
{
    public sealed partial class StaffManagementPage : Page
    {
        public StaffManagementViewModel ViewModel { get; }

        public StaffManagementPage()
        {
            ViewModel = App.GetService<StaffManagementViewModel>();
            this.InitializeComponent();
        }
    }
}