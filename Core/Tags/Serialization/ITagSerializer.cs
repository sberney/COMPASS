namespace COMPASS.Core.Tags.Serialization
{
  public interface ITagSerializer
  {
    void SerializeTags(FilePath tagsFile, SerializableTags tags);
  }
}
