using COMPASS.Core;
using COMPASS.Tools;
using System;
using System.CodeDom;
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
      Id = Utils.GetAvailableID(alltags.ToList<IHasId>());
      FT = filtertype;
      FilterValue = filterValue;
    }

    private readonly FilterType FT;
    public object FilterValue { get; init; }

    public override ITag GetGroup()
    {
      throw new NotImplementedException("this does not make sense for a filter tag");
    }

    public FilterType GetFilterGroup()
    {
      return FT;
    }
  }
}
