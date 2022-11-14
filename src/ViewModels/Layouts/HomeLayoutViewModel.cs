﻿using COMPASS.Models;
using COMPASS.Tools;

namespace COMPASS.ViewModels
{
  internal class HomeLayoutViewModel : LayoutViewModel
  {
    public HomeLayoutViewModel() : base()
    {
      LayoutType = Enums.CodexLayout.HomeLayout;

      ViewOptions.Add(new MyMenuItem("Cover Size", value => TileWidth = (double)value) { Prop = TileWidth });
      ViewOptions.Add(new MyMenuItem("Show Title", value => ShowTitle = (bool)value) { Prop = ShowTitle });
    }

    private double _width = Properties.Settings.Default.HomeCoverSize;
    public double TileWidth
    {
      get => _width;
      set
      {
        _ = SetProperty(ref _width, value);
        RaisePropertyChanged(nameof(TileHeight));
        Properties.Settings.Default.HomeCoverSize = value;
      }
    }

    public double TileHeight => (int)(_width * 4 / 3);

    private bool _showtitle = Properties.Settings.Default.HomeShowTitle;
    public bool ShowTitle
    {
      get => _showtitle;
      set
      {
        _ = SetProperty(ref _showtitle, value);
        Properties.Settings.Default.HomeShowTitle = value;
      }
    }
  }
}
