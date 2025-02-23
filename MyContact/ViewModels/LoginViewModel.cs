using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Services;
using MyContact.View;

namespace MyContact.ViewModels
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
            if (parameter is not PasswordBox passwordBox)
            {
                MessageBox.Show("Erreur lors de la récupération du mot de passe.");
                return;
            }

            string password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez entrer un email et un mot de passe.");
                return;
            }

            var user = await _usersService.AuthenticateUser(Email, password);

            if (user == null)
            {
                MessageBox.Show("Email ou mot de passe incorrect.");
                return;
            }

            // 🔹 Vérifie si l'utilisateur est admin
            if (user.Roles == 0) // 0 = Admin
            {
                MessageBox.Show("Connexion réussie !");

                // 🔹 Ouvrir AdminWindow
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();

                // 🔹 Fermer la fenêtre de connexion
                CloseLoginWindow();
            }
            else
            {
                MessageBox.Show("Accès refusé. Vous n'êtes pas administrateur.");
            }
        }

        private void CloseLoginWindow()
        {
            // 🔹 Trouve et ferme la fenêtre de connexion
            var loginWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
            loginWindow?.Close();
        }
    }
}
