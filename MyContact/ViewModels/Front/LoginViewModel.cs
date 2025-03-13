using System.Linq;
using System.Windows;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Services;
using MyContact.View;
using System.Threading.Tasks;

namespace MyContact.ViewModels.Front
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly UsersService _usersService;
        private string _email;
        private string _password;
        private string _secretCode;

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _usersService = new UsersService("http://localhost:5110");
            LoginCommand = new RelayCommand(async (parameter) => await Login());

            // Champ email préremplis
            Email = "admin@blocalimentation.fr";
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

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string SecretCode
        {
            get => _secretCode;
            set
            {
                _secretCode = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadPassword()
        {
            var user = await _usersService.GetUserByEmail(Email);
            if (user != null)
            {
                Password = "azerty";
            }
        }

        public async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(SecretCode))
            {
                MessageBox.Show("Veuillez entrer un email, un mot de passe et un code secret.");
                return;
            }

            var user = await _usersService.AuthenticateUser(Email, Password, SecretCode);

            if (user == null)
            {
                MessageBox.Show("Email, mot de passe ou code secret incorrect.");
                return;
            }

            if (user.Roles == 0)
            {
                MessageBox.Show("Connexion réussie !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();

                CloseLoginWindow();
            }
            else
            {
                MessageBox.Show("Accès refusé. Vous n'êtes pas administrateur.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CloseLoginWindow()
        {
            var loginWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
            loginWindow?.Close();
        }
    }
}