using System.Windows;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.View
{
    public partial class AddServiceWindow : Window
    {
        private readonly ServicesService _servicesService;

        public AddServiceWindow()
        {
            InitializeComponent();
            _servicesService = new ServicesService();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            string serviceName = ServiceNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                MessageBox.Show("Veuillez entrer un nom de service.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newService = new ServicesModel { Nom = serviceName };
            var result = await _servicesService.AddServiceAsync(newService);

            if (result)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Erreur lors de l'ajout du service.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
