﻿using System.Collections.Generic;

namespace COMPASS.Core
{
  public struct LoadedTags
  {
    public readonly IList<ITag> Root { get; init; }
    public readonly IList<ITag> All { get; init; }
  }
}
