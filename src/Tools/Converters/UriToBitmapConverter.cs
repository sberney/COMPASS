using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace COMPASS.Tools.Converters
{
  internal class UriToBitmapConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (File.Exists((string)value))
      {
        //Parameter is true if instant refresh is needed for Edit View, false for mass view
        bool Fullres = System.Convert.ToBoolean(parameter);
        BitmapImage bi = new();
        bi.BeginInit();

        bi.CreateOptions = Fullres ? BitmapCreateOptions.IgnoreImageCache : BitmapCreateOptions.DelayCreation;
        bi.CacheOption = BitmapCacheOption.OnLoad;
        bi.UriSource = new Uri(value.ToString());
        bi.EndInit();
        return bi;
      }
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new Exception("The method or operation is not implemented.");
    }
  }
}
