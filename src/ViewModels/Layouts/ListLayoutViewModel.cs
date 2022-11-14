﻿using COMPASS.Models;
using COMPASS.Tools;

namespace COMPASS.ViewModels
{
  public class ListLayoutViewModel : LayoutViewModel
  {
    public ListLayoutViewModel() : base()
    {
      LayoutType = Enums.CodexLayout.ListLayout;

      //MyMenuItem Columnvisibility = new MyMenuItem("Column Visibility")
      //{
      //    Submenus = new ObservableCollection<MyMenuItem>()
      //};

      //ViewOptions.Add(new MyMenuItem("Title", value => ShowTitle = (bool)value) { Prop = ShowTitle });
      ViewOptions.Add(new MyMenuItem("Author", value => ShowAuthor = (bool)value) { Prop = ShowAuthor });
      ViewOptions.Add(new MyMenuItem("Publisher", value => ShowPublisher = (bool)value) { Prop = ShowPublisher });
      ViewOptions.Add(new MyMenuItem("Release Date", value => ShowReleaseDate = (bool)value) { Prop = ShowReleaseDate });
      ViewOptions.Add(new MyMenuItem("Version", value => ShowVersion = (bool)value) { Prop = ShowVersion });
      ViewOptions.Add(new MyMenuItem("Rating", value => ShowRating = (bool)value) { Prop = ShowRating });
      ViewOptions.Add(new MyMenuItem("Tags", value => ShowTags = (bool)value) { Prop = ShowTags });
      ViewOptions.Add(new MyMenuItem("File Icons", value => ShowFileIcons = (bool)value) { Prop = ShowFileIcons });
      //ViewOptions.Add(new MyMenuItem("Edit Icon", value => ShowEditIcon = (bool)value) { Prop = ShowEditIcon });

      //ViewOptions.Add(Columnvisibility);
    }

    #region ViewOptions

    private bool _showTitle = true;
    public bool ShowTitle
    {
      get => _showTitle;
      set => SetProperty(ref _showTitle, value);
    }

    private bool _showAuthor = Properties.Settings.Default.ListShowAuthor;
    public bool ShowAuthor
    {
      get => _showAuthor;
      set
      {
        _ = SetProperty(ref _showAuthor, value);
        Properties.Settings.Default.ListShowAuthor = value;
      }
    }

    private bool _showPublisher = Properties.Settings.Default.ListShowPublisher;
    public bool ShowPublisher
    {
      get => _showPublisher;
      set
      {
        _ = SetProperty(ref _showPublisher, value);
        Properties.Settings.Default.ListShowPublisher = value;
      }
    }

    private bool _showReleaseDate = Properties.Settings.Default.ListShowRelease;
    public bool ShowReleaseDate
    {
      get => _showReleaseDate;
      set
      {
        _ = SetProperty(ref _showReleaseDate, value);
        Properties.Settings.Default.ListShowRelease = value;
      }
    }

    private bool _showVersion = Properties.Settings.Default.ListShowVersion;
    public bool ShowVersion
    {
      get => _showVersion;
      set
      {
        _ = SetProperty(ref _showVersion, value);
        Properties.Settings.Default.ListShowVersion = value;
      }
    }

    private bool _showRating = Properties.Settings.Default.ListShowRating;
    public bool ShowRating
    {
      get => _showRating;
      set
      {
        _ = SetProperty(ref _showRating, value);
        Properties.Settings.Default.ListShowRating = value;
      }
    }

    private bool _showTags = Properties.Settings.Default.ListShowTags;
    public bool ShowTags
    {
      get => _showTags;
      set
      {
        _ = SetProperty(ref _showTags, value);
        Properties.Settings.Default.ListShowTags = value;
      }
    }

    private bool _showFileIcons = Properties.Settings.Default.ListShowFileIcons;
    public bool ShowFileIcons
    {
      get => _showFileIcons;
      set
      {
        _ = SetProperty(ref _showFileIcons, value);
        Properties.Settings.Default.ListShowFileIcons = value;
      }
    }

    private bool _showEditIcon = Properties.Settings.Default.ListShowEditIcon;
    public bool ShowEditIcon
    {
      get => _showEditIcon;
      set
      {
        _ = SetProperty(ref _showEditIcon, value);
        Properties.Settings.Default.ListShowEditIcon = value;
      }
    }

    #endregion

  }
}
