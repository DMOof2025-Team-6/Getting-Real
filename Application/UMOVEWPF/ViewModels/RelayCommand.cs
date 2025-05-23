using System;
using System.Windows.Input;

namespace UMOVEWPF.ViewModels
{
    /// <summary>
    /// Implementering af ICommand der gør det muligt at binde kommandoer til UI elementer
    /// Denne klasse bruges til at håndtere brugerinteraktioner i UI
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Opretter en ny RelayCommand
        /// </summary>
        /// <param name="execute">Den handling der skal udføres når kommandoen aktiveres</param>
        /// <param name="canExecute">Funktion der bestemmer om kommandoen kan udføres</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Event der udløses når CanExecute status ændres
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Bestemmer om kommandoen kan udføres
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen</param>
        /// <returns>True hvis kommandoen kan udføres, ellers false</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Udfører kommandoen
        /// </summary>
        /// <param name="parameter">Parameter til kommandoen</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
} 