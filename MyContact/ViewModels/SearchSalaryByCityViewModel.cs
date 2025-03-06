using System.Collections.ObjectModel;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.ViewModels
{
    public class SearchSalaryByCityViewModel : ViewModelBase
    {
        private readonly SitesService _sitesService;
        private string _cityName;
        private string _resultText;
        private ObservableCollection<Salaries> _salaries;

        public SearchSalaryByCityViewModel()
        {
            _sitesService = new SitesService();
            SearchCommand = new RelayCommand(SearchSalaryByCity);
            _salaries = new ObservableCollection<Salaries>();
        }

        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
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

        private async void SearchSalaryByCity(object parameter)
        {
            if (string.IsNullOrWhiteSpace(CityName))
            {
                ResultText = "Veuillez entrer un nom de ville.";
                return;
            }

            try
            {
                string formattedCityName = char.ToUpper(CityName[0]) + CityName.Substring(1).ToLower();
                var salaries = await _sitesService.GetSalariesByCityAsync(formattedCityName);

                Salaries.Clear(); // Nettoyage avant ajout de nouveaux résultats

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
