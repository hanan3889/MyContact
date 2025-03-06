using System.Windows;
using MyContact.ViewModels;

namespace MyContact.Views
{
    /// <summary>
    /// Interaction logic for SearchSalaryView.xaml
    /// </summary>
    public partial class SearchSalaryView : Window
    {
        public SearchSalaryView()
        {
            InitializeComponent();
            DataContext = new SearchSalaryViewModel();
        }
    }
}
