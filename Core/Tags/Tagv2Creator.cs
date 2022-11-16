using COMPASS.Core.Tags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace COMPASS.Core
{
  public class Tagv2Creator : ITagCreator
  {
    protected FreshIdCreator FreshIdCreator = new();
    protected IList<ITag> ExistingTags;

    public Tagv2Creator(ITagContext tagContext)
    {
      ExistingTags = tagContext.AllTags;
    }

    public ITag CreateCopyOf(ITag source)
    {
      var tag = new Tagv2(source.Id)
      {
        Content = source.Content,
        ParentId = source.ParentId,
        IsGroup = source.IsGroup,
        BackgroundColor = source.BackgroundColor,
        Children = new ObservableCollection<ITag>(source.Children),
      };

      return tag;
    }

    public ITag CreateFresh()
    {
      return new Tagv2(FreshIdCreator.Create(ExistingTags));
    }
  }
}
