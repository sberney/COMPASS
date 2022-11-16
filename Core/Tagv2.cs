using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace COMPASS.Core
{
  public class Tagv2 : ObservableObject, IHasId, IHasChildren<ITag>, ITag
  {
    public Tagv2(TagId tagId)
    {
      TagId = tagId.Value;
    }

    private ObservableCollection<ITag> ChildrenInternal = new();
    public ObservableCollection<ITag> Children
    {
      get => ChildrenInternal;
      set => SetProperty(ref ChildrenInternal, value);
    }

    private string ContentInternal = "";
    public string Content
    {
      get => ContentInternal;
      set => SetProperty(ref ContentInternal, value);
    }

    private int ParentIdInternal = -1;
    public int ParentId
    {
      get => ParentIdInternal;
      set => SetProperty(ref ParentIdInternal, value);
    }

    private Color BackgroundColorInternal = Colors.Black;
    public Color BackgroundColor
    {
      get => BackgroundColorInternal;
      set => SetProperty(ref BackgroundColorInternal, value);
    }

    private bool IsGroupInternal;
    public bool IsGroup
    {
      get => IsGroupInternal;
      set => SetProperty(ref IsGroupInternal, value);
    }

    protected TagId TagId { get; set; }
    public int Id { get => TagId; set => TagId = value; }

    //can't save parent itself, would cause infinite loop when serializing
    public ITag GetParent()
    {
      throw new NotImplementedException("get parent must be implemented in conjunction with TagContext");
      // return ParentId == -1 ? null : AllTags.First(tag => tag.ID == ParentId);
    }

    /// <summary>
    /// If this tag is considered a group, returns this tag.
    /// If this tag is a member of any group, retreives the first such parent tag.
    /// </summary>
    /// <returns>the first parent tag that is flagged as a group</returns>
    public virtual ITag? GetGroup()  // todo: must be implemented in conjunction with TagContext
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
    public void CopyFrom(ITag source, IList<ITag> allTags) // todo: must be implemented in conjunction with TagContext
    {
      TagId = source.Id;
      Content = source.Content;
      ParentId = source.ParentId;
      IsGroup = source.IsGroup;
      BackgroundColor = source.BackgroundColor;
      Children = new ObservableCollection<ITag>(source.Children);
      // AllTags = allTags; // todo -- create TagWithContext.CopyFrom that does the equivalent
    }

    public override bool Equals(object? obj)
    {
      return obj != null && obj is ITag objAsTag && Equals(objAsTag);
    }

    public bool Equals(ITag? other)
    {
      return other != null && TagId.Equals(other.Id);
    }

    public override int GetHashCode()
    {
      return TagId;
    }
    #endregion
  }
}
