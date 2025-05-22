using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UMOVEWPF.ViewModels
{
    /// <summary>
    /// Basis-ViewModel klasse der implementerer grundlæggende funktionalitet for databinding
    /// Denne klasse implementerer INotifyPropertyChanged for at understøtte databinding i UI
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event der udløses når en egenskab ændres
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Hjælpemetode til at opdatere en egenskab og udløse PropertyChanged eventet
        /// </summary>
        /// <typeparam name="T">Typen af egenskaben</typeparam>
        /// <param name="field">Referencen til det private felt</param>
        /// <param name="value">Den nye værdi</param>
        /// <param name="propertyName">Navnet på egenskaben</param>
        /// <returns>True hvis værdien blev ændret, ellers false</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Hjælpemetode til at udløse PropertyChanged eventet
        /// </summary>
        /// <param name="propertyName">Navnet på den ændrede egenskab</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 