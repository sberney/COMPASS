namespace COMPASS.Core.Tags
{
  public interface ITagCreator
  {
    ITag CreateFresh();
    ITag CreateCopyOf(ITag source);
  }
}