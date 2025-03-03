using MahApps.Metro.Controls;
using MyContact.Models;
using MyContact.ViewModel;

namespace MyContact.View
{
    public partial class EditSalaryWindow : MetroWindow
    {
        public EditSalaryWindow(Salaries salary)
        {
            InitializeComponent();
            var viewModel = new EditSalaryViewModel(salary);
            viewModel.OnSaveCompleted += (sender, success) =>
            {
                DialogResult = success;
                Close();
            };
            DataContext = viewModel;
        }
    }
}
