namespace COMPASS.Core
{
  public interface ITagCreator<TColor>
  {
    ITag<TColor> CreateFresh();
    ITag<TColor> CreateCopyFrom(ITag<TColor> source);
  }
}