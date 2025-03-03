using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.ViewModel
{
    public class EditSalaryViewModel : INotifyPropertyChanged
    {
        private readonly SalariesService _salariesService;
        private Salaries _salary;

        public Salaries Salary
        {
            get => _salary;
            set
            {
                _salary = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditSalaryViewModel(Salaries salary)
        {
            _salariesService = new SalariesService();
            Salary = salary;

            SaveCommand = new RelayCommand(async (param) => await Save(), (param) => CanSave());
            CancelCommand = new RelayCommand((param) => Cancel());

        }

        private async Task Save()
        {
            bool success = await _salariesService.UpdateSalaryAsync(Salary);
            if (success)
            {
                OnSaveCompleted?.Invoke(this, true);
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Salary.Nom) && !string.IsNullOrWhiteSpace(Salary.Email);

        private void Cancel()
        {
            OnSaveCompleted?.Invoke(this, false);
        }

        public event System.Action<object, bool> OnSaveCompleted;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
