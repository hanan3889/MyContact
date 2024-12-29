using System.Collections.ObjectModel;
using System.Windows.Input;
using MyContact.Models;
using MyContact.Services;
using MyContact.Commands;

namespace MyContact.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly SalariesService _salariesService;
        public ObservableCollection<Salaries> Salaries { get; set; }

        public ICommand LoadSalariesCommand { get; }

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
