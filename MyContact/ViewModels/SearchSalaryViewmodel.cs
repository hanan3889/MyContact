using System.Collections.Generic;
using System.Windows.Input;
using MyContact.Models;
using MyContact.Services;
using MyContact.Commands;

namespace MyContact.ViewModels
{
    public class SearchSalaryViewModel : ViewModelBase
    {
        private readonly SalariesService _salariesService;
        private string _salaryName;
        private string _resultText;
   


        public SearchSalaryViewModel()
        {
            _salariesService = new SalariesService();
            SearchCommand = new RelayCommand(SearchSalary);
 
        }

        public string SalaryName
        {
            get => _salaryName;
            set
            {
                _salaryName = value;
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

        public ICommand SearchCommand { get; }

        private async void SearchSalary(object parameter)
        {
            try
            {
                var salaries = await _salariesService.GetSalariesByNameAsync(SalaryName);
                if (salaries != null && salaries.Count > 0)
                {
                    var result = new List<string>();
                    foreach (var salary in salaries)
                    {
                        result.Add($"Nom: {salary.Nom}\nPrénom: {salary.Prenom}\nTéléphone Fixe: {salary.TelephoneFixe}\nTéléphone Portable: {salary.TelephonePortable}\nEmail: {salary.Email}\nService: {salary.Service?.Nom}\nVille: {salary.Site?.Ville}");
                    }
                    ResultText = string.Join("\n\n", result);
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
