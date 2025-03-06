using System.Windows;
using MyContact.ViewModels;

namespace MyContact.Views
{
    public partial class SearchSalaryByCityView : Window
    {
        public SearchSalaryByCityView()
        {
            InitializeComponent();
            DataContext = new SearchSalaryByCityViewModel();
        }
    }
}
