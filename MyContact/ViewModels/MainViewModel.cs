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
        public ICommand AddSalaryCommand { get; }
        public ICommand UpdateSalaryCommand { get; }
        public ICommand DeleteSalaryCommand { get; }

        public MainViewModel()
        {
            _salariesService = new SalariesService();
            Salaries = new ObservableCollection<Salaries>();
            LoadSalariesCommand = new RelayCommand(LoadSalaries);
            AddSalaryCommand = new RelayCommand(AddSalary);
            UpdateSalaryCommand = new RelayCommand(UpdateSalary);
            DeleteSalaryCommand = new RelayCommand(DeleteSalary);
            LoadSalariesCommand.Execute(null);
        }

        private async void LoadSalaries(object parameter)
        {
            var salaries = await _salariesService.GetSalariesAsync();
            Salaries.Clear();
            foreach (var salary in salaries)
            {
                Salaries.Add(salary);
            }
        }

        private async void AddSalary(object parameter)
        {
            var newSalary = new Salaries { Nom = "New Name", Prenom = "New Last Name", TelephonePortable = "123456789" };
            await _salariesService.CreateSalaryAsync(newSalary);
            Salaries.Add(newSalary);
        }

        private async void UpdateSalary(object parameter)
        {
            if (parameter is Salaries salary)
            {
                await _salariesService.UpdateSalaryAsync(salary.Id, salary);
                // Update the UI if necessary
            }
        }

        private async void DeleteSalary(object parameter)
        {
            if (parameter is Salaries salary)
            {
                await _salariesService.DeleteSalaryAsync(salary.Id);
                Salaries.Remove(salary);
            }
        }
    }
}
