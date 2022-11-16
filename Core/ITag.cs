using System.Collections.ObjectModel;
using System.Windows.Media;

namespace COMPASS.Core
{
  public interface ITag : IHasID, IHasChildren<ITag>
  {
    Color BackgroundColor { get; set; }
    string Content { get; set; }
    bool IsGroup { get; set; }
    int ParentID { get; set; }

    // void Copy(ITag t);
    bool Equals(object obj);
    bool Equals(ITag other);
    object GetGroup();
    int GetHashCode();
    ITag GetParent();
  }
}