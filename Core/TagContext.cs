using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPASS.Core
{
  public class TagContext : ITagContext
  {
    public IList<ITag> RootTags { get; set; }
    public IList<ITag> AllTags { get; set; }
  }
}
