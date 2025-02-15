using MahApps.Metro.Controls;
using MyContact.ViewModels;

namespace MyContact.View
{
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
}
