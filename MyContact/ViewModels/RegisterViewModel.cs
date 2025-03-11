using System;
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
        private string _password;
        private string _confirmPassword;
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

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
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
            _usersService = new UsersService("http://localhost:5110");
            RegisterCommand = new RelayCommand(Register);
        }

        private async void Register(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(SecretCode))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas.");
                return;
            }

            // Vérification du format du SecretCode (4 chiffres)
            if (SecretCode.Length != 4 || !int.TryParse(SecretCode, out _))
            {
                MessageBox.Show("Le code secret doit être un nombre à 4 chiffres.");
                return;
            }


            try
            {
                bool isRegistered = await _usersService.RegisterUser(Email, Password, SecretCode);

                if (isRegistered)
                {
                    MessageBox.Show("Enregistrement réussi !");

                    // Vérifiez le rôle de l'utilisateur
                    var user = await _usersService.AuthenticateUser(Email, Password, SecretCode);
                    if (user != null && user.Roles == 1) 
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
