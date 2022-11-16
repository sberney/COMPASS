using System.Collections.Generic;

namespace COMPASS.Core
{
  public interface ITagContext
  {
    IList<ITag> AllTags { get; set; }
    IList<ITag> RootTags { get; set; }
  }
}