using System.Collections.ObjectModel;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;
using System;
using System.Threading.Tasks;

namespace MyContact.ViewModels
{
    public class SearchSalaryByServiceViewModel : ViewModelBase
    {
        private readonly ServicesService _servicesService;
        private ObservableCollection<ServicesModel> _services;
        private ServicesModel _selectedService;
        private string _resultText;
        private ObservableCollection<Salaries> _salaries;

        public SearchSalaryByServiceViewModel()
        {
            _servicesService = new ServicesService();
            SearchCommand = new RelayCommand(SearchSalaryByService);
            _salaries = new ObservableCollection<Salaries>();
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
                Salaries.Clear();

                if (salaries != null && salaries.Count > 0)
                {
                    foreach (var salary in salaries)
                    {
                        Salaries.Add(salary);
                    }
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
    }
}
