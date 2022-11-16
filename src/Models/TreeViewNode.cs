using COMPASS.Core;
using COMPASS.Core.Tags;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace COMPASS.Models
{
  public class TreeViewNode : ObservableObject, IHasChildren<TreeViewNode>
  {
    public TreeViewNode(ITag t)
    {
      Tag = t;
      Expanded = t.IsGroup;
    }

    private ITag _tag;
    public ITag Tag
    {
      get => _tag;
      set => SetProperty(ref _tag, value);
    }

    private ObservableCollection<TreeViewNode> _children = new();
    public ObservableCollection<TreeViewNode> Children
    {
      get => _children;
      set => SetProperty(ref _children, value);
    }
    IReadOnlyList<TreeViewNode> IHasChildren<TreeViewNode>.Children => _children;

    private bool _selected = false;
    public bool Selected
    {
      get => _selected;
      set => SetProperty(ref _selected, value);
    }

    private bool _expanded;
    public bool Expanded
    {
      get => _expanded;
      set => SetProperty(ref _expanded, value);
    }
  }
}
