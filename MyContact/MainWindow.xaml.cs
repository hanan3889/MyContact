using System.Windows;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.View;
using MyContact.ViewModels.Front;
using MyContact.ViewModels;
using MyContact.Views;

namespace MyContact
{
    public partial class MainWindow : Window
    {
        public ICommand OpenSearchSalaryViewCommand { get; }
        public ICommand OpenSearchSalaryByCityViewCommand { get; }
        public ICommand OpenSearchSalaryByServiceViewCommand { get; }
        public ICommand OpenLoginCommand { get; }
        public ICommand OpenRegisterCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel
            {
                OpenSearchSalaryViewCommand = new RelayCommand(OpenSearchSalaryView),
                OpenSearchSalaryByCityViewCommand = new RelayCommand(OpenSearchSalaryByCityView),
                OpenSearchSalaryByServiceViewCommand = new RelayCommand(OpenSearchSalaryByServiceView),
                OpenLoginCommand = new RelayCommand(OpenLogin),
                OpenRegisterCommand = new RelayCommand(OpenRegister)
            };

            // Ajouter de l evenement pour les touches
            this.KeyDown += MainWindow_KeyDown;
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Si CTRL + P est pressé
            if (e.Key == Key.P && Keyboard.Modifiers == ModifierKeys.Control)
            {
                // Rendre le bouton visible
                LoginButton.Visibility = Visibility.Visible;
            }
        }

        private void OpenLogin(object? obj)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void OpenRegister(object? obj)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
        }

        private void OpenSearchSalaryView(object parameter)
        {
            var searchSalaryView = new SearchSalaryView
            {
                DataContext = new SearchSalaryViewModel()
            };
            searchSalaryView.Show();
        }

        private void OpenSearchSalaryByCityView(object parameter)
        {
            var searchSalaryByCityView = new SearchSalaryByCityView
            {
                DataContext = new SearchSalaryByCityViewModel()
            };
            searchSalaryByCityView.Show();
        }

        private void OpenSearchSalaryByServiceView(object parameter)
        {
            var searchSalaryByServiceView = new SearchSalaryByServiceView
            {
                DataContext = new SearchSalaryByServiceViewModel()
            };
            searchSalaryByServiceView.Show();
        }
    }
}
