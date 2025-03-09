using System.Windows;
using System.Windows.Controls;
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginViewModel = DataContext as LoginViewModel;
            loginViewModel?.LoginCommand.Execute(new[] { PasswordInput.Password, SecretCodeInput.Password });
        }
    }
}
