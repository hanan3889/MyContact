using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyContact.Models;
using MyContact.Services;
using MyContact.Commands;
using MyContact.ViewModels;
using System.Windows.Controls;

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

            if (salaries != null && salaries.Count > 0)
            {
                var result = new List<string>();

                foreach (var salary in salaries)
                {
                    string serviceNom = salary.SiteId != 1 ? "Production" : salary.ServiceNom;
                    string villeNom = salary.SiteVille ?? formattedCityName;

                    result.Add($"Nom : {salary.Nom}\nPrénom : {salary.Prenom}\nTéléphone Fixe : {salary.TelephoneFixe}\nTéléphone Portable : {salary.TelephonePortable}\nEmail : {salary.Email}\nService : {serviceNom}\nVille : {villeNom}");
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
