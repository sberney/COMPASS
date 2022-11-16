namespace COMPASS.Core
{
  public interface ITagCreator<TColor>
  {
    ITag CreateFresh();
    ITag CreateCopyFrom(ITag source);
  }
}