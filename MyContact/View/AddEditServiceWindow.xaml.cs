using System;
using System.Windows;
using MahApps.Metro.Controls;
using MyContact.ViewModels;

namespace MyContact.View
{
    public partial class AddEditServiceWindow : MetroWindow
    {
        public AddEditServiceWindow()
        {
            InitializeComponent();
            this.DataContext = new ServicesViewModel();
        }
    }
}
