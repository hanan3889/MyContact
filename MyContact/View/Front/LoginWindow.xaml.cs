using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MyContact.ViewModels.Front;

namespace MyContact.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            try
            {
                InitializeComponent();
                DataContext = new LoginViewModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'initialisation : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var loginViewModel = DataContext as LoginViewModel;
            if (loginViewModel != null)
            {
                await loginViewModel.LoadPassword();
                PasswordInput.Password = loginViewModel.Password;
            }

            // Définir le focus sur le champ de saisie du code secret
            SecretCodeInput.Focus();
        }

        private async void SecretCodeInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var loginViewModel = DataContext as LoginViewModel;
                if (loginViewModel != null)
                {
                    loginViewModel.Password = PasswordInput.Password;
                    loginViewModel.SecretCode = SecretCodeInput.Password;

                    if (int.TryParse(loginViewModel.SecretCode, out int secretCode) && secretCode == 2325)
                    {
                        await loginViewModel.Login();
                    }
                    else
                    {
                        MessageBox.Show("Code secret incorrect.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}