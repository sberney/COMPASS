namespace COMPASS.Core.Tags
{
  public interface ITagSerializer
  {
    void SerializeTags(FilePath tagsFile, SerializableTags tags);
  }
}
