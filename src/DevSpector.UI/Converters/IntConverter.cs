using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace DevSpector.Desktop.UI.Converters
{
    public class IntConverter : IValueConverter
    {
        // Converts FROM INT to stsring
        public object Convert(object integer, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)integer).ToString();
        }

        // Convets FROM STRING to int
        public object ConvertBack(object text, Type targetType, object parameter, CultureInfo culture)
        {

            int result;

            try
            {
                result = int.Parse(text as string, NumberStyles.Integer);
            }
            catch
            {
                result = 0;
            }

            return result;
        }
    }
}
