using System.Windows;
using MyContact.ViewModels.Front;

namespace MyContact.Views
{
    /// <summary>
    /// Logique d'interaction pour SearchSalaryByServiceView.xaml
    /// </summary>
    public partial class SearchSalaryByServiceView : Window
    {
        public SearchSalaryByServiceView()
        {
            InitializeComponent();
            DataContext = new SearchSalaryByServiceViewModel();
        }
    }
}
