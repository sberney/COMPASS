using COMPASS.Core;
using COMPASS.Tools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;

namespace COMPASS.Models
{
  public class Tag : ObservableObject, IHasID, IHasChildren<ITag>, ITag
  {
    // Empty Contructor needed for serialization
    public Tag() { }

    public Tag(IList<ITag> alltags)
    {
      AllTags = alltags;
      var creator = new FreshIdCreator();
      ID = creator.Create(alltags);
    }

    //needed to get parent tag from parent ID
    [XmlIgnoreAttribute]
    public IList<ITag> AllTags;

    private ObservableCollection<ITag> _childeren = new();
    public ObservableCollection<ITag> Children
    {
      get => _childeren;
      set => SetProperty(ref _childeren, value);
    }

    private string _content = "";
    public string Content
    {
      get => _content;
      set => SetProperty(ref _content, value);
    }

    private int _parentID = -1;
    public int ParentID
    {
      get => _parentID;
      set => SetProperty(ref _parentID, value);
    }

    private Color _backgroundColor = Colors.Black;
    public Color BackgroundColor
    {
      get => _backgroundColor;
      set => SetProperty(ref _backgroundColor, value);
    }

    private bool _isGroup;
    public bool IsGroup
    {
      get => _isGroup;
      set => SetProperty(ref _isGroup, value);
    }

    public int ID { get; set; }

    //can't save parent itself, would cause infinite loop when serializing
    public ITag GetParent()
    {
      return ParentID == -1 ? null : AllTags.First(tag => tag.ID == ParentID);
    }

    //returns the first parent that is a group or null if no parents are group
    public virtual object GetGroup()
    {
      if (IsGroup)
      {
        return this;
      }

      if (ParentID == -1)
      {
        return null;
      }

      var temp = GetParent();
      while (!temp.IsGroup)
      {
        if (temp.ParentID != -1)
        {
          temp = temp.GetParent();
        }
        else
        {
          break;
        }
      }
      return temp;
    }

    #region Equal and Copy Fucntions
    public void CopyFrom(ITag source, IList<ITag> allTags)
    {
      ID = source.ID;
      Content = source.Content;
      ParentID = source.ParentID;
      IsGroup = source.IsGroup;
      BackgroundColor = source.BackgroundColor;
      Children = new ObservableCollection<ITag>(source.Children);
      AllTags = allTags;
    }

    public override bool Equals(object obj)
    {
      return obj != null && obj is ITag objAsTag && Equals(objAsTag);
    }

    public bool Equals(ITag other)
    {
      return other != null && ID.Equals(other.ID);
    }

    public override int GetHashCode()
    {
      return ID;
    }
    #endregion
  }
}
