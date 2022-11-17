using COMPASS.Core;
using COMPASS.Core.Tags;
using COMPASS.Tools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;

namespace COMPASS.Models
{
  public class Tag : ObservableObject, IHasId, IHasChildren<ITag>, ITag
  {
    // Empty Contructor needed for serialization
    public Tag() { }

    public Tag(IList<ITag> alltags)
    {
      AllTags = alltags;
      var creator = new FreshIdCreator();
      Id = creator.Create(alltags);
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
    IReadOnlyList<ITag> IHasChildren<ITag>.Children => _childeren;
    public void AddChild(ITag child)
    {
      Children.Add(child);
    }

    public void RemoveChild(ITag child)
    {
      Children.Remove(child);
    }

    public void ClearChildren()
    {
      Children.Clear();
    }



    private string _content = "";
    public string Content
    {
      get => _content;
      set => SetProperty(ref _content, value);
    }

    private int _parentID = -1;
    public int ParentId
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

    public int Id { get; set; }

    //can't save parent itself, would cause infinite loop when serializing
    public ITag GetParent()
    {
      return ParentId == -1 ? null : AllTags.First(tag => tag.Id == ParentId);
    }

    //returns the first parent that is a group or null if no parents are group
    public virtual ITag GetGroup()
    {
      if (IsGroup)
      {
        return this;
      }

      if (ParentId == -1)
      {
        return null;
      }

      var temp = GetParent();
      while (!temp.IsGroup)
      {
        if (temp.ParentId != -1)
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
      Id = source.Id;
      Content = source.Content;
      ParentId = source.ParentId;
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
      return other != null && Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
      return Id;
    }
    #endregion
  }
}
