using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Threading.Tasks;
using MyContact.View;
using MyContact.ViewModels;
using System.Xml.Linq;

public class ServicesViewModel : ViewModelBase
{
    private readonly ServicesService _servicesService;
    private ObservableCollection<ServicesModel> _services;

    public ObservableCollection<ServicesModel> Services
    {
        get => _services;
        set
        {
            _services = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddServiceCommand { get; }
    public ICommand EditServiceCommand { get; }
    public ICommand DeleteServiceCommand { get; }
    public string Nom { get; internal set; }

    public ServicesViewModel()
    {
        _servicesService = new ServicesService();
        _services = new ObservableCollection<ServicesModel>();
        LoadServices();

        AddServiceCommand = new RelayCommand(async (_) => await AddService());
        EditServiceCommand = new RelayCommand(async (param) => await UpdateService(param as ServicesModel));
        DeleteServiceCommand = new RelayCommand(async (param) => await DeleteService(param as ServicesModel));
    }

    private async Task DeleteService(ServicesModel? service)
    {
        if (service == null)
        {
            MessageBox.Show("Service non trouvé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MessageBox.Show(
        $"Êtes-vous sûr de vouloir supprimer le service '{service.Nom}' ?",
        "Confirmation de suppression", MessageBoxButton.OK, MessageBoxImage.Warning);

        var result = await _servicesService.DeleteServiceAsync(service.Id);
        if (result)
        {
            Services.Remove(service);
            MessageBox.Show("Service supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Erreur lors de la suppression du service.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task UpdateService(ServicesModel? service)
    {
        if (service == null)
        {
            MessageBox.Show("Service non trouvé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        
        AddServiceWindow editServiceWindow = new AddServiceWindow(service);
        bool? result = editServiceWindow.ShowDialog();

        if (result == true)
        {
            MessageBox.Show("Service modifié avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadServices(); 
        }
    }

    private async void LoadServices()
    {
        try
        {
            var services = await _servicesService.GetAllServicesAsync();
            if (services != null)
            {
                Services = new ObservableCollection<ServicesModel>(services);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors du chargement des services : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task AddService()
    {
        // Ouvrir la fenêtre AddServiceWindow
        AddServiceWindow addServiceWindow = new AddServiceWindow();
        addServiceWindow.ShowDialog();

        // Si l'utilisateur a ajouté un service, recharger les services
        LoadServices();
    }
}
