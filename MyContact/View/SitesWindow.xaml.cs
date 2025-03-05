using System.Windows;
using MyContact.ViewModels;
using MyContact.Models;

namespace MyContact.View
{
    public partial class SitesWindow : Window
    {
        private readonly SitesViewModel _viewModel;

        public SitesWindow()
        {
            InitializeComponent();
            _viewModel = new SitesViewModel();
            DataContext = _viewModel;
        }

        
        private void AddSiteButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditSiteWindow();
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                var newSite = addWindow.Site;
                _viewModel.Sites.Add(newSite); 
            }
        }

        
        private void EditSiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SitesDataGrid.SelectedItem is Sites selectedSite)
            {
                var editWindow = new AddEditSiteWindow(selectedSite);
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    var updatedSite = editWindow.Site;
                    var index = _viewModel.Sites.IndexOf(selectedSite);
                    if (index >= 0)
                    {
                        _viewModel.Sites[index] = updatedSite; 
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à modifier.");
            }
        }

        // Supprimer un site sélectionné
        private void DeleteSiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SitesDataGrid.SelectedItem is Sites selectedSite)
            {
                var result = MessageBox.Show($"Voulez-vous vraiment supprimer {selectedSite.Ville} ?",
                                             "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.Sites.Remove(selectedSite); // Supprimer le site de la liste
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à supprimer.");
            }
        }
    }
}
