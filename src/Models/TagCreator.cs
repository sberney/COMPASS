﻿using COMPASS.Core.Tags;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace COMPASS.Models
{
  public class TagCreator : ITagCreator
  {
    protected IList<ITag> ExistingTags;
    public TagCreator(IList<ITag> existingTags)
    {
      ExistingTags = existingTags;
    }

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
