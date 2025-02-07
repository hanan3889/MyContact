using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MyContact.Models;
using MyContact.Services;
using MyContact.Commands;
using MyContact.ViewModels;

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
        Debug.WriteLine($"🔎 Recherche en cours pour la ville : {CityName}");

        if (string.IsNullOrWhiteSpace(CityName))
        {
            ResultText = "Veuillez entrer un nom de ville.";
            return;
        }

        try
        {
            var salariesList = await _sitesService.GetSalariesByCityAsync(CityName);
            Debug.WriteLine($"✅ {salariesList.Count} salarié(s) trouvés.");

            if (salariesList != null && salariesList.Count > 0)
            {
                Salaries.Clear();  // On vide la liste avant d'ajouter de nouveaux résultats
                foreach (var salary in salariesList)
                {
                    Debug.WriteLine($"📌 Ajout : {salary.Nom} - {salary.Service?.Nom ?? "Service NULL"} - {salary.Site?.Ville ?? "Ville NULL"}");
                    Salaries.Add(salary);
                }

                ResultText = $"{salariesList.Count} salarié(s) trouvé(s).";
            }
            else
            {
                Salaries.Clear();
                ResultText = "Aucun salarié trouvé.";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Erreur : {ex.Message}");
            ResultText = $"Erreur: {ex.Message}";
        }
    }
}
