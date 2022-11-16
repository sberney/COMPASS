using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace COMPASS.Core.Tags
{
  public static class ITagContextExtensions
  {
    public static ITag? FindParentTagOf(this ITagContext context, ITag tag)
    {
      if (tag.ParentId == -1)
        return null;

      return context.AllTags.First(tag => tag.Id == tag.ParentId);
    }

    /// <summary>
    /// If this tag is considered a group, returns this tag.
    /// If this tag is a descendent of any tag that is considered a group,
    /// retrieves the most recent ancestor that is considered a group.
    /// Otherwise, retrieves the root-most parent tag that is not considered a group
    /// by walking the parent tag hierarchy.
    /// </summary>
    /// <returns>the first parent tag that is flagged as a group</returns>
    public static ITag? FindGroupOf(this ITagContext context, ITag tag)
    {
      if (tag.IsGroup)
        return tag;

      // walks up the tree (todo: optimize the list searches)
      var ancestor = context.FindParentTagOf(tag);
      if (ancestor == null)
        return null;

      // finds either a group or a root-most non-group tag, or null
      while (!ancestor.IsGroup)
      {
        var ancestorParent = context.FindParentTagOf(ancestor);
        if (ancestorParent == null)
          break;
        ancestor = ancestorParent;
      }

      return ancestor;
    }

    public static ITagCreator GetCreator(this ITagContext context)
    {
      return new Tagv2Creator(context);
    }
  }
}
