using CB.Wpf.Media;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GC.Programmer
{
    [ValueConversion(typeof(Color), typeof(ColorBrightness)), ValueConversion(typeof(string), typeof(ColorBrightness))]
    public class ColorBrighnessConverter: IValueConverter
    {
        #region Methods
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Color)
            {
                return Convert((Color)value);
            }

            if (value is string)
            {
                return Convert((Color)ColorConverter.ConvertFromString(value as string));
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        } 
        #endregion


        #region Implementation
        private ColorBrightness Convert(Color color)
        {
            var brighness = ColorHelper.CalculateBrightness(color);
            return brighness > 2.0 / 3.0 ? ColorBrightness.Bright : brighness > 1.0 / 3.0 ? ColorBrightness.Medium : ColorBrightness.Dark;
        }
        #endregion
    }
}
