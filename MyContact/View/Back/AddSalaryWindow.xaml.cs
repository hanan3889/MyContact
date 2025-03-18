using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.View
{
    public partial class AddSalaryWindow : Window, INotifyPropertyChanged
    {
        private readonly SalariesService _salariesService;

        public ObservableCollection<ServicesModel> Services { get; set; } = new();
        public ObservableCollection<Sites> Sites { get; set; } = new();

        private string _nom = "";
        public string Nom
        {
            get => _nom;
            set
            {
                _nom = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
                UpdateEmail();
            }
        }

        private string _prenom = "";
        public string Prenom
        {
            get => _prenom;
            set
            {
                _prenom = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
                UpdateEmail();
            }
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _telephoneFixe = "";
        public string TelephoneFixe
        {
            get => _telephoneFixe;
            set
            {
                if (IsNumeric(value) || string.IsNullOrEmpty(value))
                {
                    _telephoneFixe = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));
                }
                else
                {
                    MessageBox.Show("Le numéro de téléphone fixe doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private string _telephonePortable = "";
        public string TelephonePortable
        {
            get => _telephonePortable;
            set
            {
                if (IsNumeric(value) || string.IsNullOrEmpty(value))
                {
                    _telephonePortable = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));
                }
                else
                {
                    MessageBox.Show("Le numéro de téléphone portable doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public bool IsValid => !string.IsNullOrWhiteSpace(Nom) &&
                               !string.IsNullOrWhiteSpace(Prenom) &&
                               (IsValidPhoneNumber(TelephoneFixe) || IsValidPhoneNumber(TelephonePortable));

        private ServicesModel? _selectedService;
        public ServicesModel? SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                OnPropertyChanged();
            }
        }

        private Sites? _selectedSite;
        public Sites? SelectedSite
        {
            get => _selectedSite;
            set
            {
                _selectedSite = value;
                OnPropertyChanged();
            }
        }

        public AddSalaryWindow()
        {
            InitializeComponent();
            _salariesService = new SalariesService();
            DataContext = this;
            _ = LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var services = await _salariesService.GetServicesAsync();
                var sites = await _salariesService.GetSitesAsync();

                Services.Clear();
                foreach (var service in services) Services.Add(service);
                Sites.Clear();
                foreach (var siteItem in sites) Sites.Add(siteItem);

                SelectedService = Services.FirstOrDefault();
                SelectedSite = Sites.FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateEmail()
        {
            if (!string.IsNullOrWhiteSpace(Nom) && !string.IsNullOrWhiteSpace(Prenom))
            {
                Email = $"{Prenom.ToLower()}.{Nom.ToLower()}@blocalimentation.fr";
            }
        }

        private async void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedService == null || SelectedSite == null)
            {
                MessageBox.Show("Veuillez sélectionner un service et un site.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newSalary = new Salaries
            {
                Nom = Nom,
                Prenom = Prenom,
                Email = Email,
                TelephoneFixe = TelephoneFixe,
                TelephonePortable = TelephonePortable,
                ServiceId = SelectedService.Id,
                SiteId = SelectedSite.Id
            };

            bool success = await _salariesService.CreateSalaryAsync(newSalary);

            if (success)
            {
                MessageBox.Show("Salarié ajouté avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Erreur lors de l'ajout du salarié.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && Regex.IsMatch(phoneNumber, "^\\d+$");
        }

        private bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, "^\\d*$");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
