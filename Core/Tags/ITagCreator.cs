namespace COMPASS.Core.Tags
{
  public interface ITagCreator<TColor>
  {
    ITag CreateFresh();
    ITag CreateCopyFrom(ITag source);
  }
}