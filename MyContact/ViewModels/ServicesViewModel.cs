using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.ViewModels;

public class ServicesViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:7140/api/Services";

    private ObservableCollection<Services> _services;
    public ObservableCollection<Services> Services
    {
        get => _services;
        set
        {
            _services = value;
            OnPropertyChanged();
        }
    }

    private Services _selectedService;
    public Services SelectedService
    {
        get => _selectedService;
        set
        {
            _selectedService = value;
            OnPropertyChanged();
        }
    }

    public ICommand EditServiceCommand { get; }
    public ICommand DeleteServiceCommand { get; }

    public ServicesViewModel()
    {
        _httpClient = new HttpClient();
        _services = new ObservableCollection<Services>();
        LoadServices();

        EditServiceCommand = new RelayCommand(async (param) => await UpdateService(param as Services));
        DeleteServiceCommand = new RelayCommand(async (param) => await DeleteService(param as Services));
    }

    private async void LoadServices()
    {
        try
        {
            var services = await _httpClient.GetFromJsonAsync<ObservableCollection<Services>>(BaseUrl);
            if (services != null)
            {
                Services = services;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors du chargement des services : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task UpdateService(Services service)
    {
        if (service == null)
        {
            MessageBox.Show("Veuillez sélectionner un service à modifier.", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{service.Id}", service);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Service mis à jour avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadServices(); // Rafraîchir la liste
            }
            else
            {
                MessageBox.Show("Erreur lors de la mise à jour du service.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Exception : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task DeleteService(Services service)
    {
        if (service == null)
        {
            MessageBox.Show("Veuillez sélectionner un service à supprimer.", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var result = MessageBox.Show($"Voulez-vous vraiment supprimer le service \"{service.Nom}\" ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes)
            return;

        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{service.Id}");

            if (response.IsSuccessStatusCode)
            {
                Services.Remove(service);
                MessageBox.Show("Service supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Erreur lors de la suppression du service.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Exception : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
