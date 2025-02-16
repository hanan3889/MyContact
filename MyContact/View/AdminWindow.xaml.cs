using MyContact.ViewModels;
using System.Windows;

namespace MyContact.View
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            DataContext = new AdminViewModel(); 
        }
    }
}
