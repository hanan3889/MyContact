using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MyContact.Commands;
using MyContact.Models;
using MyContact.Services;

namespace MyContact.ViewModels
{
    public class EditSiteViewModel : INotifyPropertyChanged
    {
        private readonly SitesService _sitesService;
        private Sites _site;

        public Sites Site
        {
            get => _site;
            set
            {
                _site = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditSiteViewModel(Sites site)
        {
            _sitesService = new SitesService();
            Site = site;

            SaveCommand = new RelayCommand(async (param) => await Save(), (param) => CanSave());
            CancelCommand = new RelayCommand((param) => Cancel());
        }

        private async Task Save()
        {
            bool success = await _sitesService.UpdateSiteAsync(Site);
            if (success)
            {
                OnSaveCompleted?.Invoke(this, true);
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Site.Ville);

        private void Cancel()
        {
            OnSaveCompleted?.Invoke(this, false);
        }

        public event System.Action<object, bool> OnSaveCompleted;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
