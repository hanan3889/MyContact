using System.Windows;
using System.Windows.Controls;
using MyContact.ViewModels;

namespace MyContact.View
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            var viewModel = new RegisterViewModel();
            viewModel.CurrentWindow = this;
            DataContext = viewModel;
        }

        private void PasswordInput_Checked(object sender, RoutedEventArgs e)
        {
            if (PasswordInput != null && PasswordTextBox != null)
            {
                PasswordInput.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordTextBox.Text = PasswordInput.Password;
            }
        }

        private void PasswordInput_Unchecked(object sender, RoutedEventArgs e)
        {
            if (PasswordInput != null && PasswordTextBox != null)
            {
                PasswordInput.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordInput.Password = PasswordTextBox.Text;
            }
        }

        private void ConfirmPasswordInput_Checked(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordInput != null && ConfirmPasswordTextBox != null)
            {
                ConfirmPasswordInput.Visibility = Visibility.Collapsed;
                ConfirmPasswordTextBox.Visibility = Visibility.Visible;
                ConfirmPasswordTextBox.Text = ConfirmPasswordInput.Password;
            }
        }

        private void ConfirmPasswordInput_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordInput != null && ConfirmPasswordTextBox != null)
            {
                ConfirmPasswordInput.Visibility = Visibility.Visible;
                ConfirmPasswordTextBox.Visibility = Visibility.Collapsed;
                ConfirmPasswordInput.Password = ConfirmPasswordTextBox.Text;
            }
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm && PasswordInput != null)
            {
                vm.Password = PasswordInput.Password;
            }
        }

        private void ConfirmPasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm && ConfirmPasswordInput != null)
            {
                vm.ConfirmPassword = ConfirmPasswordInput.Password;
            }
        }

        private void SecretCodeInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm && SecretCodeInput != null)
            {
                vm.SecretCode = SecretCodeInput.Password;
            }
        }
    }
}
