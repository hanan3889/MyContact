using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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


                if (services.Count == 0 || sites.Count == 0)
                {
                    MessageBox.Show("Aucune donnée récupérée. Vérifiez l'API.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Services.Clear();
                foreach (var service in services)
                {
                    Services.Add(service);
                }

                Sites.Clear();
                foreach (var siteItem in sites)
                {
                    Sites.Add(siteItem);
                }

                SelectedService = Services.FirstOrDefault();
                SelectedSite = Sites.FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedService == null || SelectedSite == null)
            {
                MessageBox.Show("Veuillez sélectionner un service et un site.");
                return;
            }

            var newSalary = new Salaries
            {
                Nom = NomTextBox.Text,
                Prenom = PrenomTextBox.Text,
                Email = EmailTextBox.Text,
                TelephoneFixe = TelephoneFixeTextBox.Text,
                TelephonePortable = TelephonePortableTextBox.Text,
                ServiceId = SelectedService.Id, 
                SiteId = SelectedSite.Id 
            };

            bool success = await _salariesService.CreateSalaryAsync(newSalary);

            if (success)
            {
                MessageBox.Show("Salarié ajouté avec succès !");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Erreur lors de l'ajout du salarié.");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
