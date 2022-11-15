﻿using COMPASS.Core;
using COMPASS.Tools;
using System.Collections.ObjectModel;
using System.Linq;
using static COMPASS.Tools.Enums;

namespace COMPASS.Models
{
  public class FilterTag : Tag
  {
    public FilterTag() : base() { }
    public FilterTag(ObservableCollection<FilterTag> alltags, FilterType filtertype, object filterValue = null)
    {
      ID = Utils.GetAvailableID(alltags.ToList<IHasID>());
      FT = filtertype;
      FilterValue = filterValue;
    }

    private readonly FilterType FT;
    public object FilterValue { get; init; }

    public override object GetGroup()
    {
      return FT;
    }
  }
}
