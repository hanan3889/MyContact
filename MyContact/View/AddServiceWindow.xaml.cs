using System.Windows;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.View
{
    public partial class AddServiceWindow : Window
    {
        private readonly ServicesService _servicesService;
        private readonly ServicesModel? _existingService;

        public string ServiceName { get; private set; } = string.Empty;
        public int? ServiceId { get; private set; }

        public AddServiceWindow(ServicesModel? service = null)
        {
            InitializeComponent();
            _servicesService = new ServicesService();
            _existingService = service;

            if (_existingService != null)
            {
                //Préremplir les champs si modification
                ServiceNameTextBox.Text = _existingService.Nom;
                ConfirmButton.Content = "Modifier";
                ServiceId = _existingService.Id;
            }
            else
            {
                ConfirmButton.Content = "Ajouter";
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            ServiceName = ServiceNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(ServiceName))
            {
                MessageBox.Show("Veuillez entrer un nom de service valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_existingService == null)
            {

                var newService = new ServicesModel { Nom = ServiceName };
                var result = await _servicesService.AddServiceAsync(newService);

                if (!result)
                {
                    MessageBox.Show("Erreur lors de l'ajout du service.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {

                _existingService.Nom = ServiceName;
                var result = await _servicesService.UpdateServiceAsync(_existingService);

                if (!result)
                {
                    MessageBox.Show("Erreur lors de la mise à jour du service.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
