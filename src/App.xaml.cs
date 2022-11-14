using COMPASS.Models;
using System.IO;
using System.Windows;

namespace COMPASS
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
      _ = Directory.CreateDirectory(Constants.CompassDataPath);
    }
  }
}
