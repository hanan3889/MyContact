using System.Windows;
using MyContact.Models;
using MyContact.Services;
using MyContact.ViewModels.Front;

namespace MyContact.View
{
    public partial class SitesWindow : Window
    {
        private readonly SitesViewModel _viewModel;
        private readonly SitesService _sitesService;

        public SitesWindow()
        {
            InitializeComponent();
            _viewModel = new SitesViewModel();
            _sitesService = new SitesService();
            DataContext = _viewModel;
        }

        private async void AddSiteButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditSiteWindow();
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                var newSite = addWindow.Site;
                bool success = await _sitesService.AddSiteAsync(newSite);

                if (success)
                {
                    _viewModel.Sites.Add(newSite);
                    MessageBox.Show("Site ajouté avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du site.");
                }
            }
        }

        private async void EditSiteButton_Click(object sender, RoutedEventArgs e)
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
                        bool success = await _sitesService.UpdateSiteAsync(updatedSite);

                        if (success)
                        {
                            _viewModel.Sites[index] = updatedSite;
                            MessageBox.Show("Site modifié avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la mise à jour du site.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à modifier.");
            }
        }

        // Supprimer un site sélectionné
        private async void DeleteSiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SitesDataGrid.SelectedItem is Sites selectedSite)
            {
                var result = MessageBox.Show($"Voulez-vous vraiment supprimer {selectedSite.Ville} ?",
                                             "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = await _sitesService.DeleteSiteAsync(selectedSite.Id);

                    if (success)
                    {
                        _viewModel.Sites.Remove(selectedSite);
                        MessageBox.Show("Site supprimé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression du site.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à supprimer.");
            }
        }
    }
}