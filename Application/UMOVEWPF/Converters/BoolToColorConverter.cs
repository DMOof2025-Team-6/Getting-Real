using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UMOVEWPF.Converters
{
    /// <summary>
    /// Konverterer en boolsk værdi til en farve
    /// Denne konverter bruges til at vise forskellige farver baseret på en boolsk tilstand
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        /// <summary>
        /// Konverterer en boolsk værdi til en farve
        /// </summary>
        /// <param name="value">Den boolske værdi der skal konverteres</param>
        /// <param name="targetType">Måltypen (ikke brugt)</param>
        /// <param name="parameter">Ekstra parameter (ikke brugt)</param>
        /// <param name="culture">Kultur information (ikke brugt)</param>
        /// <returns>Rød farve hvis true, grøn hvis false, grå hvis ikke boolsk</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// Konverterer en farve tilbage til en boolsk værdi (ikke implementeret)
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 