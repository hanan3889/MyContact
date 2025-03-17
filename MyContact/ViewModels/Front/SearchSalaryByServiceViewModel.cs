using System.Collections.ObjectModel;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyContact.ViewModels.Front
{
    public class SearchSalaryByServiceViewModel : ViewModelBase
    {
        private readonly ServicesService _servicesService;
        private ObservableCollection<ServicesModel> _services;
        private ServicesModel _selectedService;
        private string _resultText;
        private string _searchText;
        private ObservableCollection<Salaries> _salaries;
        private ObservableCollection<Salaries> _allSalaries; // Stocke tous les résultats avant filtrage

        public SearchSalaryByServiceViewModel()
        {
            _servicesService = new ServicesService();
            SearchCommand = new RelayCommand(SearchSalaryByService);
            _salaries = new ObservableCollection<Salaries>();
            _allSalaries = new ObservableCollection<Salaries>(); // Initialise la liste complète
            _services = new ObservableCollection<ServicesModel>();
            LoadServices();
        }

        public ObservableCollection<ServicesModel> Services
        {
            get => _services;
            set
            {
                _services = value;
                OnPropertyChanged();
            }
        }

        public ServicesModel SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                OnPropertyChanged();
            }
        }

        public string ResultText
        {
            get => _resultText;
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterSalaries(); // Filtrer les salariés en temps réel
            }
        }

        public ObservableCollection<Salaries> Salaries
        {
            get => _salaries;
            set
            {
                _salaries = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }

        private async void LoadServices()
        {
            try
            {
                var services = await _servicesService.GetAllServicesAsync();
                Services = new ObservableCollection<ServicesModel>(services);
            }
            catch (Exception ex)
            {
                ResultText = $"Erreur lors du chargement des services: {ex.Message}";
            }
        }

        private async void SearchSalaryByService(object parameter)
        {
            if (SelectedService == null)
            {
                ResultText = "Veuillez sélectionner un service.";
                return;
            }

            try
            {
                var salaries = await _servicesService.GetSalariesByServiceNameAsync(SelectedService.Nom);
                _allSalaries.Clear();
                Salaries.Clear();

                if (salaries != null && salaries.Count > 0)
                {
                    foreach (var salary in salaries)
                    {
                        _allSalaries.Add(salary); // Stocke tous les résultats
                    }
                    FilterSalaries(); // Appliquer le filtre initial
                    ResultText = $"{salaries.Count} salarié(s) trouvé(s).";
                }
                else
                {
                    ResultText = "Aucun salarié trouvé.";
                }
            }
            catch (Exception ex)
            {
                ResultText = $"Erreur: {ex.Message}";
            }
        }

        private void FilterSalaries()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Salaries = new ObservableCollection<Salaries>(_allSalaries);
            }
            else
            {
                Salaries = new ObservableCollection<Salaries>(
                    _allSalaries.Where(s =>
                        s.Nom.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        s.Prenom.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        s.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    ));
            }
        }
    }
}
