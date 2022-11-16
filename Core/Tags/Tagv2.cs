using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace COMPASS.Core.Tags
{
  public class Tagv2 : ObservableObject, IHasId, IHasChildren<ITag>, ITag
  {
    private TagId TagId;
    private ObservableCollection<ITag> ChildrenInternal = new();
    private string ContentInternal = "";
    private int ParentIdInternal = -1;
    private Color BackgroundColorInternal = Colors.Black;
    private bool IsGroupInternal;

    /// <summary>
    /// Creates a new tag using the indicated Id
    /// </summary>
    /// <param name="tagId">Globally unique ID representing this tag uniquely</param>
    public Tagv2(TagId tagId)
    {
      TagId = tagId.Value;
    }

    /// <summary>
    /// DO NOT USE.
    /// Required for XmlSerializer automated data population.
    /// </summary>
    protected Tagv2() { }

    public int Id { get => TagId; set => SetProperty(ref TagId, value); }

    public ObservableCollection<ITag> Children
    {
      get => ChildrenInternal;
      set => SetProperty(ref ChildrenInternal, value);
    }

    public string Content
    {
      get => ContentInternal;
      set => SetProperty(ref ContentInternal, value);
    }

    public int ParentId
    {
      get => ParentIdInternal;
      set => SetProperty(ref ParentIdInternal, value);
    }

    public Color BackgroundColor
    {
      get => BackgroundColorInternal;
      set => SetProperty(ref BackgroundColorInternal, value);
    }

    public bool IsGroup
    {
      get => IsGroupInternal;
      set => SetProperty(ref IsGroupInternal, value);
    }

    #region Equal and Copy Fucntions

    public override bool Equals(object? obj)
    {
      return obj != null && obj is ITag objAsTag && Equals(objAsTag);
    }

    /// <summary>
    /// Compares the Id of two ITag instances to determine value equality
    /// </summary>
    /// <param name="other">Target ITag to test equality against</param>
    /// <returns>Whether the two instances represent the same keyed entity</returns>
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
