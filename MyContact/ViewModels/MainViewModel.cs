using System.Collections.ObjectModel;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly SalariesService _salariesService;
        public ObservableCollection<Salaries> Salaries { get; set; }

        public ICommand LoadSalariesCommand { get; }
        public Commands.RelayCommand OpenSearchSalaryViewCommand { get; internal set; }
        public Commands.RelayCommand OpenSearchSalaryByCityViewCommand { get; internal set; }
        public RelayCommand OpenSearchSalaryByServiceViewCommand { get; internal set; }
        public RelayCommand OpenRegisterCommand { get; internal set; }
        public RelayCommand OpenLoginCommand { get; internal set; }

        public MainViewModel()
        {
            _salariesService = new SalariesService();
            Salaries = new ObservableCollection<Salaries>();
            LoadSalariesCommand = new RelayCommand(LoadSalaries);
            LoadSalariesCommand.Execute(null);
        }

        private async void LoadSalaries(object? parameter)
        {
            var salaries = await _salariesService.GetSalariesAsync();
            Salaries.Clear();
            foreach (var salary in salaries)
            {
                Salaries.Add(salary);
            }
        }
    }
}
