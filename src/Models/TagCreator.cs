using COMPASS.Core.Tags;
using System.Collections.Generic;

namespace COMPASS.Models
{
  public class TagCreator : ITagCreator
  {
    protected ITagContext TagContext => new TagContext();
    protected IList<ITag> ExistingTags => TagContext.AllTags;

    public ITag CreateCopyOf(ITag source)
    {
      var tag = new Tag(ExistingTags);
      tag.CopyFrom(source, ExistingTags);
      return tag;
    }

    public ITag CreateFresh()
    {
      return new Tag(ExistingTags);
    }
  }
}
