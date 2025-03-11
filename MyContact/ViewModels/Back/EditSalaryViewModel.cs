using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.ViewModels.Back
{
    public class EditSalaryViewModel : INotifyPropertyChanged
    {
        private readonly SalariesService _salariesService;
        private Salaries _salary;
        private Salaries _editedSalary;

        public Salaries Salary
        {
            get => _salary;
            private set
            {
                _salary = value;
                OnPropertyChanged();
            }
        }

        public Salaries EditedSalary
        {
            get => _editedSalary;
            set
            {
                _editedSalary = value;
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

            // Copier l'objet pour éviter la modification directe
            EditedSalary = new Salaries
            {
                Id = salary.Id,
                Nom = salary.Nom,
                Prenom = salary.Prenom,
                TelephoneFixe = salary.TelephoneFixe,
                TelephonePortable = salary.TelephonePortable,
                Email = salary.Email
            };

            SaveCommand = new RelayCommand(async (param) => await Save(), (param) => CanSave());
            CancelCommand = new RelayCommand((param) => Cancel());
        }

        private async Task Save()
        {
            // Copier les modifications vers Salary avant d'enregistrer
            Salary.Nom = EditedSalary.Nom;
            Salary.Prenom = EditedSalary.Prenom;
            Salary.TelephoneFixe = EditedSalary.TelephoneFixe;
            Salary.TelephonePortable = EditedSalary.TelephonePortable;
            Salary.Email = EditedSalary.Email;

            bool success = await _salariesService.UpdateSalaryAsync(Salary);
            if (success)
            {
                OnSaveCompleted?.Invoke(this, true);
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(EditedSalary.Nom) && !string.IsNullOrWhiteSpace(EditedSalary.Email);

        private void Cancel()
        {
            OnSaveCompleted?.Invoke(this, false);
        }

        public event Action<object, bool> OnSaveCompleted;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
