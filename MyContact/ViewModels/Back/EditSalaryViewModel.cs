using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
                if (_selectedService != null)
                {
                    EditedSalary.ServiceId = _selectedService.Id;
                }
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
                if (_selectedSite != null)
                {
                    EditedSalary.SiteId = _selectedSite.Id;
                }
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

            // Copie pour ne pas modifier directement Salary
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

            SaveCommand = new RelayCommand(async (param) => await Save());
            CancelCommand = new RelayCommand((param) => Cancel());

            _ = LoadData();
        }

        private async Task LoadData()
        {
            var services = await _salariesService.GetServicesAsync();
            var sites = await _salariesService.GetSitesAsync();

            Services.Clear();
            foreach (var service in services)
            {
                Services.Add(service);
            }

            Sites.Clear();
            foreach (var site in sites)
            {
                Sites.Add(site);
            }

            // Sélectionner le service en fonction de l'ID du service actuel
            SelectedService = Services.FirstOrDefault(s => s.Id == EditedSalary.ServiceId);
            SelectedSite = Sites.FirstOrDefault(s => s.Id == EditedSalary.SiteId);

            OnPropertyChanged(nameof(SelectedService));
            OnPropertyChanged(nameof(SelectedSite));
        }

        private async Task Save()
        {
            // Apply the modifications to Salary
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