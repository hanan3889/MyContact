using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.ViewModels
{
    public class SearchSalaryByCityViewModel : ViewModelBase
    {
        private readonly SitesService _sitesService;
        private string _resultText;
        private ObservableCollection<Salaries> _salaries;
        private ObservableCollection<Sites> _sites;
        private Sites _selectedSite;

        public SearchSalaryByCityViewModel()
        {
            _sitesService = new SitesService();
            SearchCommand = new RelayCommand(SearchSalaryByCity);
            _salaries = new ObservableCollection<Salaries>();
            _sites = new ObservableCollection<Sites>();
            LoadSites();
        }

        public ObservableCollection<Sites> Sites
        {
            get => _sites;
            set
            {
                _sites = value;
                OnPropertyChanged();
            }
        }

        public Sites SelectedSite
        {
            get => _selectedSite;
            set
            {
                _selectedSite = value;
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

        private async void LoadSites()
        {
            var sitesList = await _sitesService.GetSitesAsync();
            Sites = new ObservableCollection<Sites>(sitesList);
        }

        private async void SearchSalaryByCity(object parameter)
        {
            if (SelectedSite == null)
            {
                ResultText = "Veuillez sélectionner une ville.";
                return;
            }

            try
            {
                var salaries = await _sitesService.GetSalariesByCityAsync(SelectedSite.Ville);

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