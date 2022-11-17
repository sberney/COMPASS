using System.IO;

namespace COMPASS.Core.Serialization
{
  public interface IDataDeserializer<T> where T : class
  {
    T? Deserialize(TextReader reader);
  }
}