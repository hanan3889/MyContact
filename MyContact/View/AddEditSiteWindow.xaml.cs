using System.Windows;
using MahApps.Metro.Controls;
using MyContact.Models;
using MyContact.ViewModels;

namespace MyContact.View
{
    public partial class AddEditSiteWindow : MetroWindow
    {
        public EditSiteViewModel ViewModel { get; }
        public object Site { get; internal set; }

        public AddEditSiteWindow()
        {
            InitializeComponent();
            ViewModel = new EditSiteViewModel(new Sites());  
            DataContext = ViewModel;
            ConfigureEventHandlers();
        }

        
        public AddEditSiteWindow(Sites site) : this()
        {
            ViewModel = new EditSiteViewModel(site);
            DataContext = ViewModel;
        }

        private void ConfigureEventHandlers()
        {
            ViewModel.OnSaveCompleted += (sender, success) =>
            {
                DialogResult = success;
                Close();
            };
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveCommand.Execute(null);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CancelCommand.Execute(null);
        }
    }
}
