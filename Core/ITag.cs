using System.Collections.ObjectModel;
using System.Windows.Media;

namespace COMPASS.Core
{
  public interface ITag : IHasId, IHasChildren<ITag>
  {
    Color BackgroundColor { get; set; }
    string Content { get; set; }
    bool IsGroup { get; set; }
    int ParentId { get; set; }
    ObservableCollection<ITag> Children { get; set; }

    // void Copy(ITag t);
    bool Equals(object? obj);
    bool Equals(ITag? other);
    ITag? GetGroup();
    int GetHashCode();
    ITag GetParent();
  }
}