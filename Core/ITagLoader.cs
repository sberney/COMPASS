using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPASS.Core
{
  public interface ITagLoader<TColor>
  {
    LoadedTags<TColor>? LoadTags(FilePath? tagsFile);
  }
}
