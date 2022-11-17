using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using COMPASS.Core.Tags.Serialization;

namespace COMPASS.Core.Tags
{
  // todo: add template parameter for Group type if GetGroup remains on this interface
  public interface ITag : ITagCore<ITag>, IHasMutableChildren<ITag>
  {
    bool Equals(object? obj);
    bool Equals(ITag? other);
    int GetHashCode();
  }
}