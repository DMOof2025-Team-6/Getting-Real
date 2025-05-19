using System;
using System.Windows.Input;

namespace UMOVEWPF.Helpers
{
    /// <summary>
    /// Implementering af ICommand, så man kan binde knapper til metoder i ViewModel.
    /// Gør det muligt at bruge kommandoer i MVVM.
    /// </summary>
    public class RelayCommand : ICommand //ICommand er intergreret i systmet.
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Opretter en ny RelayCommand Constructor
        /// </summary>
        /// <param name="execute">Metode der skal køres når kommandoen aktiveres</param>
        /// <param name="canExecute">Valgfri metode der bestemmer om kommandoen kan aktiveres</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Returnerer true hvis kommandoen må aktiveres
        /// </summary>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter); //Hvis den er 0 kan den udføres, ellers ikke. God til brug med knapper.
        /// <summary>
        /// Udfører den tilknyttede metode
        /// </summary>
        public void Execute(object parameter) => _execute(parameter);

        /// <summary>
        /// Event der trigges når CanExecute skal genvurderes. F.eks. med en markert bus.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}