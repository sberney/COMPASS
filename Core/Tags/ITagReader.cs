using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPASS.Core.Tags
{
  public interface ITagReader
  {
    SerializableTags? ReadTags(FilePath tagsFile);
  }
}
