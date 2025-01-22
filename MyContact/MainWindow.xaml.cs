using System.Windows;
using System.Windows.Input;
using MyContact.Views;
using MyContact.ViewModels;
using MyContact.Commands;
using MahApps.Metro.Controls;

namespace MyContact
{
    public partial class MainWindow : MetroWindow
    {
        public ICommand OpenSearchSalaryViewCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(); // Assurez-vous que votre MainViewModel est correctement défini
            OpenSearchSalaryViewCommand = new RelayCommand(OpenSearchSalaryView);
            this.DataContext = this; // Assurez-vous que le DataContext est défini sur cette instance
        }

        private void OpenSearchSalaryView(object parameter)
        {
            var searchSalaryView = new SearchSalaryView
            {
                DataContext = new SearchSalaryViewModel()
            };
            searchSalaryView.Show();
        }
    }
}
