using System;
using System.Globalization;
using System.Windows.Data;

namespace UMOVEWPF.Converters
{
    /// <summary>
    /// Konverterer en enum type til en liste af dens værdier
    /// Denne konverter bruges til at vise alle mulige værdier fra en enum i UI elementer som ComboBox
    /// </summary>
    public class EnumToListConverter : IValueConverter
    {
        /// <summary>
        /// Konverterer en enum type til en array af dens værdier
        /// </summary>
        /// <param name="value">Enum typen der skal konverteres</param>
        /// <param name="targetType">Måltypen (ikke brugt)</param>
        /// <param name="parameter">Alternativ enum type hvis value ikke er en type</param>
        /// <param name="culture">Kultur information (ikke brugt)</param>
        /// <returns>Et array med alle værdier fra enum typen</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumType = value as Type;
            if (enumType == null && value != null)
                enumType = value.GetType();
            if (enumType != null && enumType.IsEnum)
                return Enum.GetValues(enumType);
            if (parameter is Type t && t.IsEnum)
                return Enum.GetValues(t);
            return null;
        }

        /// <summary>
        /// Konverterer en liste tilbage til en enum type (ikke implementeret)
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 