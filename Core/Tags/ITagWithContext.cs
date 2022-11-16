using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPASS.Core.Tags
{
  public interface ITagWithContext
  {
    ITag Tag { get; }
    ITagContext Context { get; }
  }
}
