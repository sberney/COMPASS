using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace COMPASS.Tools.Converters
{
  public class ToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      //Parameter is true if inverted
      bool Invert = System.Convert.ToBoolean(parameter);
      bool visible = value.GetType() == typeof(string) ? !string.IsNullOrEmpty((string)value) : System.Convert.ToBoolean(value);
      return visible ^ Invert ? Visibility.Visible : (object)Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
