using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.ViewModels.Back
{
    public class EditSalaryViewModel : INotifyPropertyChanged
    {
        private readonly SalariesService _salariesService;
        private Salaries _salary;
        private Salaries _editedSalary;

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

        public Salaries Salary
        {
            get => _salary;
            private set
            {
                _salary = value;
                OnPropertyChanged();
            }
        }

        public Salaries EditedSalary
        {
            get => _editedSalary;
            set
            {
                _editedSalary = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditSalaryViewModel(Salaries salary)
        {
            _salariesService = new SalariesService();
            Salary = salary;

            // Copier l'objet pour éviter la modification directe
            EditedSalary = new Salaries
            {
                Id = salary.Id,
                Nom = salary.Nom,
                Prenom = salary.Prenom,
                TelephoneFixe = salary.TelephoneFixe,
                TelephonePortable = salary.TelephonePortable,
                Email = salary.Email,
                ServiceId = salary.ServiceId,
                SiteId = salary.SiteId
            };

            SaveCommand = new RelayCommand(async (param) => await Save(), (param) => CanSave());
            CancelCommand = new RelayCommand((param) => Cancel());

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
                    // Gérer l'erreur si nécessaire
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

                SelectedService = Services.FirstOrDefault(s => s.Id == EditedSalary.ServiceId);
                SelectedSite = Sites.FirstOrDefault(s => s.Id == EditedSalary.SiteId);
            }
            catch (Exception ex)
            {
                // Gérer l'erreur si nécessaire
            }
        }

        private async Task Save()
        {
            // Copier les modifications vers Salary avant d'enregistrer
            Salary.Nom = EditedSalary.Nom;
            Salary.Prenom = EditedSalary.Prenom;
            Salary.TelephoneFixe = EditedSalary.TelephoneFixe;
            Salary.TelephonePortable = EditedSalary.TelephonePortable;
            Salary.Email = EditedSalary.Email;
            Salary.ServiceId = SelectedService?.Id ?? 0;
            Salary.SiteId = SelectedSite?.Id ?? 0;

            bool success = await _salariesService.UpdateSalaryAsync(Salary);
            if (success)
            {
                OnSaveCompleted?.Invoke(this, true);
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(EditedSalary.Nom) && !string.IsNullOrWhiteSpace(EditedSalary.Email);

        private void Cancel()
        {
            OnSaveCompleted?.Invoke(this, false);
        }

        public event Action<object, bool> OnSaveCompleted;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
