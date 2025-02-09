using MyContact.ViewModels;
using System.Windows;

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
