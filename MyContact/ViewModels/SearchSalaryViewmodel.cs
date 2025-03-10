using System.Collections.ObjectModel;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Services;
using MyContact.Models;

namespace MyContact.ViewModels
{
    public class SearchSalaryViewModel : ViewModelBase
    {
        private readonly SalariesService _salariesService;
        private string _salaryName;
        private ObservableCollection<Salaries> _filteredSalaries;
        private List<Salaries> _allSalaries;
        private bool _listVisibility;

        public SearchSalaryViewModel()
        {
            _salariesService = new SalariesService();
            _filteredSalaries = new ObservableCollection<Salaries>();
            _allSalaries = new List<Salaries>();
            _listVisibility = false;
            SearchCommand = new RelayCommand(async (_) => await LoadSalaries());
            _ = LoadSalaries(); 
        }

        public string SalaryName
        {
            get => _salaryName;
            set
            {
                _salaryName = value;
                OnPropertyChanged();
                FilterSalaries();
            }
        }

        public ObservableCollection<Salaries> FilteredSalaries
        {
            get => _filteredSalaries;
            set
            {
                _filteredSalaries = value;
                OnPropertyChanged();
            }
        }

        public bool ListVisibility
        {
            get => _listVisibility;
            set
            {
                _listVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }

        private async Task LoadSalaries()
        {
            var salaries = await _salariesService.GetSalariesAsync();
            if (salaries != null)
            {
                _allSalaries = salaries;
                FilterSalaries();
            }
        }

        private void FilterSalaries()
        {
            if (string.IsNullOrWhiteSpace(SalaryName))
            {
                FilteredSalaries.Clear();
                ListVisibility = false;
            }
            else
            {
                var filtered = _allSalaries
                    .Where(s => s.Nom.Contains(SalaryName, System.StringComparison.OrdinalIgnoreCase) ||
                                s.Prenom.Contains(SalaryName, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                FilteredSalaries = new ObservableCollection<Salaries>(filtered);
                ListVisibility = filtered.Any();
            }
        }
    }
}
