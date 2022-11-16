using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPASS.Core.Tags
{
  public interface ITagLoader
  {
    LoadedTags? LoadTags(FilePath? tagsFile);
  }
}
