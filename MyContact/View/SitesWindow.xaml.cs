using System.Windows;
using MyContact.ViewModels;
using MyContact.Models;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.View;



namespace MyContact.View
{
    public partial class SitesWindow : Window
    {
        private readonly SitesViewModel _viewModel;

        public SitesViewModel DataContext { get; }

        public SitesWindow()
        {
            InitializeComponent();
            _viewModel = new SitesViewModel();
            DataContext = _viewModel;
        }

        //Ouvrir la fenêtre d'ajout d'un site
        private void AddSiteButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditSiteWindow();
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                var newSite = addWindow.Site;
                _viewModel.AddSiteCommand.Execute(newSite);
            }
        }

        //Modifier un site sélectionné
        private void EditSiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SitesDataGrid.SelectedItem is Sites selectedSite)
            {
                var editWindow = new AddEditSiteWindow(selectedSite);
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    var updatedSite = editWindow.Site;
                    _viewModel.EditSiteCommand.Execute(updatedSite);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à modifier.");
            }
        }

        //Supprimer un site sélectionné
        private void DeleteSiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SitesDataGrid.SelectedItem is Sites selectedSite)
            {
                var result = MessageBox.Show($"Voulez-vous vraiment supprimer {selectedSite.Ville} ?",
                                             "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.DeleteSiteCommand.Execute(selectedSite);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site à supprimer.");
            }
        }
    }
}
