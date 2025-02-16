using System.Windows;
using MahApps.Metro.Controls;
using MyContact.ViewModels;

namespace MyContact.View
{
    public partial class RegisterWindow : MetroWindow
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
            PasswordInput.Visibility = Visibility.Collapsed;
            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordTextBox.Text = PasswordInput.Password;
        }

        private void PasswordInput_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordInput.Visibility = Visibility.Visible;
            PasswordTextBox.Visibility = Visibility.Collapsed;
            PasswordInput.Password = PasswordTextBox.Text;
        }
    }
}
