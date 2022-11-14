using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace COMPASS.Models
{
  public class MyMenuItem : ObservableObject
  {
    public MyMenuItem(string header, Action<object> UpdateProp = null)
    {
      Header = header;
      _updateProp = UpdateProp;
    }

    #region Properties

    private readonly Action<object> _updateProp;

    //Name of Item
    private string _header;
    public string Header
    {
      get => _header;
      set => SetProperty(ref _header, value);
    }

    //Property it changes, such as bools for toggeling options, floats for sliders, ect.
    private object _prop;
    public object Prop
    {
      get => _prop;
      set
      {
        _ = SetProperty(ref _prop, value);
        _updateProp?.Invoke(value);
      }
    }

    //Command to execute on click
    private ICommand _command;
    public ICommand Command
    {
      get => _command;
      set => SetProperty(ref _command, value);
    }

    private object _commandParam;
    public object CommandParam
    {
      get => _commandParam;
      set => SetProperty(ref _commandParam, value);
    }

    private ObservableCollection<MyMenuItem> _submenus;
    public ObservableCollection<MyMenuItem> Submenus
    {
      get => _submenus;
      set => SetProperty(ref _submenus, value);
    }
    #endregion
  }
}
