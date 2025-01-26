using System.Collections.Generic;
using System.Windows.Input;
using MyContact.Models;
using MyContact.Services;
using MyContact.Commands;

namespace MyContact.ViewModels
{
    // Gère la logique de recherche et les interactions avec la vue
    public class SearchSalaryByCityViewModel : ViewModelBase
    {
        private readonly SitesService _sitesService;
        private string _cityName;
        private string _resultText;

        public SearchSalaryByCityViewModel()
        {
            _sitesService = new SitesService();
            SearchCommand = new RelayCommand(SearchSalaryByCity);
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

        public ICommand SearchCommand { get; }

        private async void SearchSalaryByCity(object parameter)
        {
            try
            {
                var salaries = await _sitesService.GetSalariesByCityAsync(CityName);
                if (salaries != null && salaries.Count > 0)
                {
                    var result = new List<string>();
                    foreach (var salary in salaries)
                    {
                        result.Add($"Nom : {salary.Nom}\nPrénom : {salary.Prenom}\nTéléphone Fixe : {salary.TelephoneFixe}\nTéléphone Portable : {salary.TelephonePortable}\nEmail : {salary.Email}\nService : {salary.ServiceNom}\nVille : {salary.SiteVille}");
                    }
                    ResultText = string.Join("\n\n", result);
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
