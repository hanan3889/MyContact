using System.Linq;
using System.Windows;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Services;
using MyContact.View;

namespace MyContact.ViewModels.Front
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UsersService _usersService;
        private string _email;

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _usersService = new UsersService("http://localhost:5110");
            LoginCommand = new RelayCommand(async (parameter) => await Login(parameter));
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private async Task Login(object parameter)
        {
            if (parameter is string[] passwords && passwords.Length == 2)
            {
                string password = passwords[0];
                string secretCode = passwords[1];

                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(secretCode))
                {
                    MessageBox.Show("Veuillez entrer un email, un mot de passe et un code secret.");
                    return;
                }

                var user = await _usersService.AuthenticateUser(Email, password, secretCode);

                if (user == null)
                {
                    
                    MessageBox.Show("Email, mot de passe ou code secret incorrect.");
                    return;
                }

                // Vérifie si l'utilisateur est admin
                if (user.Roles == 0) // 0 = Admin
                {
                    MessageBox.Show("Connexion réussie !");

                    // Ouvrir AdminWindow
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();

                    CloseLoginWindow();
                }
                else
                {
                    MessageBox.Show("Accès refusé. Vous n'êtes pas administrateur.");
                }
            }
            else
            {
                MessageBox.Show("Erreur lors de la récupération du mot de passe ou du code secret.");
            }
        }

        private void CloseLoginWindow()
        {
            // Trouve et ferme la fenêtre de connexion
            var loginWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
            loginWindow?.Close();
        }
    }
}
