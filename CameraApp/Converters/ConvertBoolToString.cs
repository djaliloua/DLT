using System.Globalization;

namespace CameraApp.Converters
{
    public class ConvertBoolToString : IValueConverter
    {
        public string TrueString { get; set; } = "";
        public string FalseString { get; set; } = "";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return val ? TrueString : FalseString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
