using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyContact.ViewModels
{
    /// <summary>
    /// Classe de base pour les ViewModels qui implémente l'interface INotifyPropertyChanged.
    /// Cette classe permet de notifier les changements de propriétés dans les ViewModels.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Événement déclenché lorsqu'une propriété change.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Méthode protégée pour notifier les changements de propriétés.
        /// Utilise l'attribut CallerMemberName pour obtenir automatiquement le nom de la propriété qui a changé.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui a changé.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
