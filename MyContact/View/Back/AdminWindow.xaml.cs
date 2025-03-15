using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.View
{
    public partial class AdminWindow : Window
    {
        private readonly SalariesService _salariesService;
        private ObservableCollection<Salaries> _allSalaries; // Liste complète non filtrée
        public ObservableCollection<Salaries> Salaries { get; set; }

        public AdminWindow()
        {
            InitializeComponent();
            DataContext = this;
            _salariesService = new SalariesService();
            Salaries = new ObservableCollection<Salaries>();
            _allSalaries = new ObservableCollection<Salaries>();
            LoadSalaries();
        }

        private async Task LoadSalaries()
        {
            var salaries = await _salariesService.GetSalariesAsync();
            _allSalaries.Clear();
            Salaries.Clear();
            foreach (var salary in salaries)
            {
                _allSalaries.Add(salary);
                Salaries.Add(salary);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            var filteredSalaries = _allSalaries.Where(s =>
                s.Nom.ToLower().Contains(searchText) ||
                s.Prenom.ToLower().Contains(searchText) ||
                s.Email.ToLower().Contains(searchText) ||
                s.TelephoneFixe.ToLower().Contains(searchText) ||
                s.TelephonePortable.ToLower().Contains(searchText) ||
                s.SiteVille.ToLower().Contains(searchText) ||
                s.ServiceNom.ToLower().Contains(searchText)).ToList();

            Salaries.Clear();
            foreach (var salary in filteredSalaries)
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
            AddSalaryWindow addSalaryWindow = new AddSalaryWindow();
            bool? result = addSalaryWindow.ShowDialog();

            if (result == true)
            {
                await LoadSalaries();
            }
        }

        private async void EditSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Salaries salary)
            {
                EditSalaryWindow editWindow = new EditSalaryWindow(salary);
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    // Recharger les données pour s'assurer que le DataGrid est correctement mis à jour
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

        private async void DeleteSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSalaries = Salaries.Where(s => s.IsSelected).ToList();
            if (selectedSalaries.Any())
            {
                var result = MessageBox.Show($"Voulez-vous vraiment supprimer les {selectedSalaries.Count} salariés sélectionnés ?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (var salary in selectedSalaries)
                    {
                        bool success = await _salariesService.DeleteSalaryAsync(salary.Id);
                        if (!success)
                        {
                            MessageBox.Show("Erreur lors de la suppression d'un salarié.");
                            return;
                        }
                    }
                    MessageBox.Show("Salariés supprimés !");
                    await LoadSalaries();
                }
            }
            else
            {
                MessageBox.Show("Aucun salarié sélectionné.");
            }
        }

        private void SitesButton_Click(object sender, RoutedEventArgs e)
        {
            SitesWindow sitesWindow = new SitesWindow();
            sitesWindow.Show();
        }

        private void ServicesButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditServiceWindow addEditServiceWindow = new AddEditServiceWindow();
            addEditServiceWindow.ShowDialog();
        }
    }
}