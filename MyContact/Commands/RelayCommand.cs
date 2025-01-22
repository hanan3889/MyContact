using System;
using System.Windows.Input;

namespace MyContact.Commands
{
    /// <summary>
    /// Classe RelayCommand qui implémente l'interface ICommand pour gérer les commandes dans une application WPF.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Constructeur de la classe RelayCommand.
        /// </summary>
        /// <param name="execute">Délégué Action qui contient la logique à exécuter lorsque la commande est invoquée.</param>
        /// <param name="canExecute">Délégué Func optionnel qui contient la logique pour déterminer si la commande peut être exécutée.</param>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Méthode qui détermine si la commande peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>True si la commande peut être exécutée, sinon false.</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Méthode qui exécute la logique de la commande.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Événement déclenché lorsque la capacité de la commande à être exécutée change.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
