using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UMOVEWPF.Converters
{
    /// <summary>
    /// Konverterer en procentværdi til en farve for en progress bar
    /// Denne konverter bruges til at vise forskellige farver baseret på batteriniveau eller andre procentvise værdier
    /// </summary>
    public class ProgressBarColorConverter : IValueConverter
    {
        /// <summary>
        /// Konverterer en procentværdi til en farve
        /// </summary>
        /// <param name="value">Procentværdien der skal konverteres (0-100)</param>
        /// <param name="targetType">Måltypen (ikke brugt)</param>
        /// <param name="parameter">Ekstra parameter (ikke brugt)</param>
        /// <param name="culture">Kultur information (ikke brugt)</param>
        /// <returns>
        /// Grøn farve hvis niveau >= 60%
        /// Guld farve hvis niveau >= 30%
        /// Rød farve hvis niveau >= 13%
        /// Sort farve hvis niveau < 13%
        /// Grå farve hvis værdien ikke er et tal
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double level)
            {
                if (level >= 60)
                    return new SolidColorBrush(Colors.Green);
                if (level >= 30)
                    return new SolidColorBrush(Colors.Gold);
                if (level >= 13)
                    return new SolidColorBrush(Colors.Red);
                return new SolidColorBrush(Colors.Black);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// Konverterer en farve tilbage til en procentværdi (ikke implementeret)
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 