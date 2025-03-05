using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;
using MyContact.View;

namespace MyContact.ViewModels
{
    public class SitesViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;
        private readonly SitesService _sitesService;
        private ObservableCollection<Sites> _sites;

        public Sites Site { get; private set; } = new Sites();

        public ICommand LoadSitesCommand { get; }
        public ICommand AddSiteCommand { get; }
        public ICommand EditSiteCommand { get; }
        public ICommand DeleteSiteCommand { get; }

        public SitesViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri("http://localhost:5110") };
            _sitesService = new SitesService(); 
            _sites = new ObservableCollection<Sites>();

            LoadSitesCommand = new RelayCommand(async (_) => await LoadSites());
            AddSiteCommand = new RelayCommand(async (_) => await AddSite());
            EditSiteCommand = new RelayCommand<Sites>(async (site) => await EditSite(site));
            DeleteSiteCommand = new RelayCommand<Sites>(async (site) => await DeleteSite(site));

            LoadSitesCommand.Execute(null); 
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

        public object SitesService { get; internal set; }

        private async Task LoadSites()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/Sites");
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                var sites = JsonSerializer.Deserialize<ObservableCollection<Sites>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Sites = sites ?? new ObservableCollection<Sites>();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erreur de chargement des sites: {ex.Message}");
            }
        }

        private async Task AddSite()
        {
            var newSite = new Sites();
            var addWindow = new AddEditSiteWindow(newSite);
            bool? result = addWindow.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(newSite.Ville))
            {
                bool success = await _sitesService.AddSiteAsync(newSite);

                if (success)
                {
                    await LoadSites();
                    MessageBox.Show("Site ajouté avec succès !");
                }
                else
                {
                    MessageBox.Show("Échec de l'ajout du site !");
                }
            }
        }

        private async Task EditSite(Sites site)
        {
            if (site == null) return;

            var editWindow = new AddEditSiteWindow(site); 
            bool? result = editWindow.ShowDialog();

            if (result == true)
            {
                Sites updatedSite = editWindow.Site; 

                bool success = await _sitesService.UpdateSiteAsync(updatedSite); 

                if (success)
                {
                    int index = Sites.IndexOf(site);
                    if (index >= 0)
                    {
                        Sites[index] = updatedSite; 
                    }
                    MessageBox.Show("Site modifié avec succès !");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du site !");
                }
            }
        }

        private async Task DeleteSite(Sites site)
        {
            if (site == null) return;

            var result = MessageBox.Show($"Voulez-vous vraiment supprimer le site {site.Ville} ?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"/api/Sites/{site.Id}");
                    response.EnsureSuccessStatusCode();
                    Sites.Remove(site);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Erreur de suppression : {ex.Message}");
                }
            }
        }
    }
}
