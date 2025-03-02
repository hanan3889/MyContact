using MyContact.Models;
using MyContact.Services;
using MyContact.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

public class ServicesViewModel : ViewModelBase
{
    private readonly ServicesService _servicesService;
    private ObservableCollection<ServicesModel> _services; // Utilise le bon modèle 'Services'

    public ObservableCollection<ServicesModel> Services
    {
        get => _services;
        set
        {
            _services = value;
            OnPropertyChanged();
        }
    }

    public ServicesViewModel()
    {
        _servicesService = new ServicesService();
        _services = new ObservableCollection<ServicesModel>();
        _ = LoadServices();
        //LoadServices();
    }

    private async Task LoadServices()
    {
        try
        {
            // Charge les données en arrière-plan pour éviter le freeze de l'UI
            var services = await Task.Run(() => _servicesService.GetAllServicesAsync());

            if (services == null || services.Count == 0)
            {
                MessageBox.Show("⚠️ Aucun service trouvé dans l'API !", "Alerte", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Mise à jour de l'UI (ObservableCollection) doit se faire sur le thread principal
            Application.Current.Dispatcher.Invoke(() =>
            {
                Services.Clear();
                foreach (var service in services)
                {
                    Services.Add(service);
                }
            });

            MessageBox.Show($"✅ {services.Count} services récupérés avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"❌ Erreur lors de la récupération des services : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


}
