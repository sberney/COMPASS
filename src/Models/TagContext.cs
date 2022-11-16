using COMPASS.Core.Tags;
using COMPASS.ViewModels;
using System.Collections.Generic;

namespace COMPASS.Models
{
  public class TagContext : ITagContext
  {
    public IList<ITag> RootTags => ViewModelBase.MVM.CurrentCollection.RootTags;
    public IList<ITag> AllTags => ViewModelBase.MVM.CurrentCollection.AllTags;
  }
}
