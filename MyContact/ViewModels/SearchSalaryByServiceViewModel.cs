using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyContact.Models;
using MyContact.Services;
using MyContact.Commands;
using MyContact.ViewModels;

namespace MyContact.ViewModels
{
    public class SearchSalaryByServiceViewModel : ViewModelBase
    {
        private readonly ServicesService _servicesService;
        private string _serviceName;
        private string _resultText;
        private ObservableCollection<Salaries> _salaries;

        public SearchSalaryByServiceViewModel()
        {
            _servicesService = new ServicesService();
            SearchCommand = new RelayCommand(SearchSalaryByService);
            _salaries = new ObservableCollection<Salaries>();
        }

        public string ServiceName
        {
            get => _serviceName;
            set
            {
                _serviceName = value;
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

        private async void SearchSalaryByService(object parameter)
        {
            if (string.IsNullOrWhiteSpace(ServiceName))
            {
                ResultText = "Veuillez entrer un nom de service.";
                return;
            }

            try
            {
                string formattedServiceName = char.ToUpper(ServiceName[0]) + ServiceName.Substring(1).ToLower();
                var salaries = await _servicesService.GetSalariesByServiceNameAsync(formattedServiceName);

                if (salaries != null && salaries.Count > 0)
                {
                    var result = new List<string>();

                    foreach (var salary in salaries)
                    {
                        string serviceNom = salary.ServiceNom;
                        string villeNom = salary.SiteVille;

                        result.Add($"Nom : {salary.Nom}\nService : {serviceNom}\nVille : {villeNom}");
                    }

                    ResultText = string.Join("\n\n", result);
                }
                else
                {
                    Salaries.Clear();
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
