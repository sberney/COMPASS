using System.Collections.ObjectModel;

namespace COMPASS.Core
{
  public interface ITag<TColor> : IHasID, IHasChildren<ITag<TColor>>
  {
    TColor BackgroundColor { get; set; }
    string Content { get; set; }
    bool IsGroup { get; set; }
    int ParentID { get; set; }

    // void Copy(ITag t);
    bool Equals(object obj);
    bool Equals(ITag<TColor> other);
    object GetGroup();
    int GetHashCode();
    ITag<TColor> GetParent();
  }
}