using System.Windows;
using MahApps.Metro.Controls;
using MyContact.ViewModels;

namespace MyContact
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
