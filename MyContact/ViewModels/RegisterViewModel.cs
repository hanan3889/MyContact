using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
        private string _errorMessage;

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

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
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
            // Reset error message
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(SecretCode))
            {
                ShowError("Veuillez remplir tous les champs.");
                ClearAllFields(); // Ajout de cette ligne
                return;
            }

            if (Password != ConfirmPassword)
            {
                ShowError("Les mots de passe ne correspondent pas.");
                ClearAllFields(); // Remplacer ClearPasswordFields() par ClearAllFields()
                return;
            }

            // Vérification de la longueur du mot de passe
            if (Password.Length < 6)
            {
                ShowError("Le mot de passe doit contenir \nau moins 5 caractères.", true);
                ClearAllFields(); // Remplacer ClearPasswordFields() par ClearAllFields()
                return;
            }

            // Vérification du format du code pin
            if (SecretCode.Length != 4 || !int.TryParse(SecretCode, out _))
            {
                ShowError("Le code secret doit être \nun nombre à 4 chiffres.");
                ClearAllFields(); // Remplacer cette ligne (qui ne vidait que SecretCode)
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
                        ClearAllFields();
                    }
                }
                else
                {
                    ShowError("Erreur lors de l'enregistrement.");
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erreur lors de l'enregistrement : {ex.Message}");
                ClearAllFields();
            }
        }

        private void ShowError(string message, bool isPasswordError = false)
        {
            ErrorMessage = message;
        }

        private void ClearPasswordFields()
        {
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private void ClearAllFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            SecretCode = string.Empty;
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