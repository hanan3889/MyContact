using System;
using System.Windows;


namespace MyContact.View
{
    public partial class AddEditServiceWindow : Window
    {
        public AddEditServiceWindow()
        {
            InitializeComponent();
            this.DataContext = new ServicesViewModel();
        }
    }
}
