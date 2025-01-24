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
            DataContext = new MainViewModel(); 
            OpenSearchSalaryViewCommand = new RelayCommand(OpenSearchSalaryView);
            this.DataContext = this; 
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
