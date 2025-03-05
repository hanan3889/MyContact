using System.Windows;
using MyContact.Models;

namespace MyContact.View
{
    public partial class AddEditSiteWindow : Window
    {
        public Sites Site { get; private set; }

        public AddEditSiteWindow(Sites site = null)
        {
            InitializeComponent();
            Site = site ?? new Sites();
            DataContext = Site;  
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Site.Ville))
            {
                MessageBox.Show("Veuillez entrer un nom de ville.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
