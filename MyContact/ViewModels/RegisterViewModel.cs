using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Services;

namespace MyContact.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly UsersService _usersService;

        public ICommand RegisterCommand { get; }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public RegisterViewModel()
        {
            _usersService = new UsersService("https://localhost:7140");
            RegisterCommand = new RelayCommand(Register);
        }

        private async void Register(object parameter)
        {
            Console.WriteLine("🔍 Register command triggered");
            MessageBox.Show("Tu as cliqué sur S'enregistrer");

            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Veuillez entrer un email.");
                return;
            }

            if (parameter is object[] parameters && parameters.Length == 2 &&
                parameters[0] is PasswordBox passwordBox && parameters[1] is PasswordBox confirmPasswordBox)
            {
                string password = passwordBox.Password;
                string confirmPassword = confirmPasswordBox.Password;

                if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    MessageBox.Show("Veuillez entrer un mot de passe.");
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Les mots de passe ne correspondent pas.");
                    return;
                }

                try
                {
                    Console.WriteLine($"🔍 Tentative d'enregistrement de {Email}...");
                    bool isRegistered = await _usersService.RegisterUser(Email, password);

                    if (isRegistered)
                    {
                        MessageBox.Show("✅ Enregistrement réussi !");
                        Console.WriteLine("✅ Inscription réussie !");
                    }
                    else
                    {
                        MessageBox.Show("❌ Erreur lors de l'enregistrement.");
                        Console.WriteLine("❌ Échec de l'enregistrement !");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}");
                    Console.WriteLine($"❌ Erreur : {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Erreur : Impossible de récupérer les champs de mot de passe.");
            }
        }
    }
}
