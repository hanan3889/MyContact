using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.View
{
    public partial class AdminWindow : Window
    {
        private readonly SalariesService _salariesService;
        public ObservableCollection<Salaries> Salaries { get; set; }

        public AdminWindow()
        {
            InitializeComponent();
            DataContext = this;
            _salariesService = new SalariesService();
            Salaries = new ObservableCollection<Salaries>();
            LoadSalaries();
        }

        private async Task LoadSalaries()
        {
            var salaries = await _salariesService.GetSalariesAsync();
            Salaries.Clear();
            foreach (var salary in salaries)
            {
                Salaries.Add(salary);
            }
        }

        private async void SalariesButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadSalaries();
        }

        private async void AddSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            var newSalary = new Salaries
            {
                Nom = "Nouveau Nom",
                Prenom = "Nouveau Prénom",
                TelephoneFixe = "123456789",
                TelephonePortable = "987654321",
                Email = "nouveau@example.com",
                SiteVille = "Nouvelle Ville",
                ServiceNom = "Nouveau Service",
                SiteId = 1,
                ServiceId = 1
            };

            await _salariesService.CreateSalaryAsync(newSalary);
            await LoadSalaries();
        }

        private async void EditSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Salaries salary)
            {
                EditSalaryWindow editWindow = new EditSalaryWindow(salary);
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    await LoadSalaries();
                }
            }
        }


        private async void DeleteSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Salaries salary)
            {
                var result = MessageBox.Show($"Voulez-vous vraiment supprimer {salary.Nom} ?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = await _salariesService.DeleteSalaryAsync(salary.Id);
                    if (success)
                    {
                        MessageBox.Show("Salarié supprimé !");
                        await LoadSalaries();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression.");
                    }
                }
            }
        }
    }
}
