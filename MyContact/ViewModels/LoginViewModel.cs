using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.View;

namespace MyContact.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object parameter)
        {
            string correctPassword = "admin123"; 
            if (((PasswordBox)parameter).Password == correctPassword)
            {
                MessageBox.Show("Connexion réussie !");
                new AdminWindow().Show();
                Application.Current.Windows[1]?.Close();
            }
            else
            {
                MessageBox.Show("Mot de passe incorrect.");
            }
        }
    }
}
