﻿using System.Windows;
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
        public ICommand OpenSearchSalaryByCityViewCommand { get; }
        public ICommand OpenSearchSalaryByServiceViewCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            OpenSearchSalaryViewCommand = new RelayCommand(OpenSearchSalaryView);
            OpenSearchSalaryByCityViewCommand = new RelayCommand(OpenSearchSalaryByCityView);
            OpenSearchSalaryByServiceViewCommand = new RelayCommand(OpenSearchSalaryByServiceView);

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
