using System.Windows;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Services;
using MyContact.View;

namespace MyContact.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly UsersService _usersService;
        public ICommand RegisterCommand { get; }

        private string _email;
        private string _secretCode;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string SecretCode
        {
            get { return _secretCode; }
            set
            {
                _secretCode = value;
                OnPropertyChanged(nameof(SecretCode));
            }
        }

        public Window CurrentWindow { get; set; }

        public RegisterViewModel()
        {
            _usersService = new UsersService("https://localhost:7140");
            RegisterCommand = new RelayCommand(Register);
        }

        private async void Register(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Veuillez entrer un email.");
                return;
            }

            if (parameter is string[] passwords && passwords.Length == 3)
            {
                string password = passwords[0];
                string confirmPassword = passwords[1];
                string secretCode = passwords[2];

                if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(secretCode))
                {
                    MessageBox.Show("Enregistrement refusé. Vous n'êtes pas administrateur.");
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Les mots de passe ne correspondent pas.");
                    return;
                }

                if (secretCode != "12345")
                {
                    MessageBox.Show("Code secret incorrect.");
                    return;
                }


                try
                {
                    bool isRegistered = await _usersService.RegisterUser(Email, password, secretCode);

                    if (isRegistered)
                    {
                        MessageBox.Show("Enregistrement réussi !");

                        // Vérifiez le rôle de l'utilisateur
                        var user = await _usersService.AuthenticateUser(Email, password, secretCode);
                        if (user != null && user.Roles == 0) 
                        {
                            OpenAdminWindow();
                        }
                        else
                        {
                            MessageBox.Show("Accès refusé. Vous n'êtes pas administrateur.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'enregistrement.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Erreur : Impossible de récupérer les champs de mot de passe.");
            }
        }

        private void OpenAdminWindow()
        {
            if (CurrentWindow != null)
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                CurrentWindow.Close();
            }
            else
            {
                MessageBox.Show("Erreur : Fenêtre actuelle introuvable.");
            }
        }
    }
}
