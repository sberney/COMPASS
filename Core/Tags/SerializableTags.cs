using System.Collections.Generic;

namespace COMPASS.Core.Tags
{
  public struct SerializableTags
  {
    public readonly IReadOnlyList<ITag> Root { get; init; }
    public readonly IReadOnlyList<ITag> All { get; init; }
  }
}
