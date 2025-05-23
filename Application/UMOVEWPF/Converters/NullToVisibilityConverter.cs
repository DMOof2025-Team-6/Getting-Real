using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UMOVEWPF.Converters
{
    /// <summary>
    /// Konverterer null værdier til synlighedsstatus
    /// Denne konverter bruges til at skjule UI elementer når deres datakilde er null
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Konverterer en værdi til en synlighedsstatus
        /// </summary>
        /// <param name="value">Værdien der skal konverteres</param>
        /// <param name="targetType">Måltypen (ikke brugt)</param>
        /// <param name="parameter">Ekstra parameter (ikke brugt)</param>
        /// <param name="culture">Kultur information (ikke brugt)</param>
        /// <returns>Visibility.Collapsed hvis værdien er null, ellers Visibility.Visible</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Konverterer en synlighedsstatus tilbage til en værdi (ikke implementeret)
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 