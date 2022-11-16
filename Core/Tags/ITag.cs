using System.Collections.ObjectModel;
using System.Windows.Media;

namespace COMPASS.Core.Tags
{
  // todo: add template parameter for Group type if GetGroup remains on this interface
  public interface ITag : IHasId, IHasChildren<ITag>
  {
    Color BackgroundColor { get; set; }
    string Content { get; set; }
    bool IsGroup { get; set; }
    int ParentId { get; set; }

    bool Equals(object? obj);
    bool Equals(ITag? other);
    int GetHashCode();
  }
}