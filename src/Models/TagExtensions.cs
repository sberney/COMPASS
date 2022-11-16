using COMPASS.Core.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPASS.Models
{
  public static class TagExtensions
  {
    public static ITag GetParent(this ITag tag, ITagContext context = null)
    {
      if (tag is Tag tagImpl)
        return tagImpl.GetParent();

      if (context == null)
        throw new ArgumentNullException(nameof(context), "For non-Tag ITag implementations, tag context is required");

      return context.FindParentTagOf(tag);
    }

    public static ITag GetGroup(this ITag tag, ITagContext context = null)
    {
      if (tag is Tag tagImpl)
        return tagImpl.GetGroup();

      if (context == null)
        throw new ArgumentNullException(nameof(context), "For non-Tag ITag implementations, tag context is required");

      return context.FindGroupOf(tag);
    }
  }
}
