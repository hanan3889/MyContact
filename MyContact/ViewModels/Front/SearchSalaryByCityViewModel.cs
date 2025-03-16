using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.ViewModels.Front
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

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterSalaries(); 
            }
        }
        private List<Salaries> _allSalaries = new List<Salaries>(); // Liste complète pour la recherche



        public ICommand SearchCommand { get; }

        private async void LoadSites()
        {
            var sitesList = await _sitesService.GetSitesAsync();
            Sites = new ObservableCollection<Sites>(sitesList);
        }

        private void FilterSalaries()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                //on affiche tous les salariés si la recherche est vide
                Salaries = new ObservableCollection<Salaries>(_allSalaries); 
            }
            else
            {
                Salaries = new ObservableCollection<Salaries>(
                    _allSalaries.Where(s => s.Nom.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                            s.Prenom.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                );
            }
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
                //on récupère les salariés par ville
                var salaries = await _sitesService.GetSalariesByCityAsync(SelectedSite.Ville);

                if (salaries != null && salaries.Count > 0)
                {
                    _allSalaries = salaries;
                    FilterSalaries(); 
                    ResultText = $"{salaries.Count} salarié(s) trouvé(s).";
                }
                else
                {
                    _allSalaries.Clear();
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