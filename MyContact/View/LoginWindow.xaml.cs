using System.Windows;
using MahApps.Metro.Controls;
using MyContact.ViewModels;

namespace MyContact.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
}
