using COMPASS.Core;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using IWinTag = COMPASS.Core.ITag<System.Windows.Media.Color>;

namespace COMPASS.Models
{
  public class TagCreator : ITagCreator<Color>
  {
    protected IList<IWinTag> ExistingTags;
    public TagCreator(IList<IWinTag> existingTags)
    {
      ExistingTags = existingTags;
    }

    public IWinTag CreateCopyFrom(IWinTag source)
    {
      var tag = new Tag(ExistingTags);
      tag.CopyFrom(source, ExistingTags);
      return tag;
    }

    public IWinTag CreateFresh()
    {
      return new Tag(ExistingTags);
    }
  }
}
