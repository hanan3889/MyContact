using System.Windows;
using System.Windows.Controls;
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
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
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

            if (parameter is PasswordBox[] passwordBoxes && passwordBoxes.Length == 2)
            {
                string password = passwordBoxes[0].Password;
                string confirmPassword = passwordBoxes[1].Password;

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
                        OpenAdminWindow();
                    }
                    else
                    {
                        MessageBox.Show("❌ Erreur lors de l'enregistrement.");
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
                MessageBox.Show("⚠️ Erreur : Fenêtre actuelle introuvable.");
            }
        }

    }
}
