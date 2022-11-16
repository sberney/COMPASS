using System.Collections.Generic;

namespace COMPASS.Core.Tags
{
  public interface ITagContext
  {
    IList<ITag> AllTags { get; }
    IList<ITag> RootTags { get; }
  }
}