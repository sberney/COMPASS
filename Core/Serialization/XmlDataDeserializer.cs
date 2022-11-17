using System.IO;
using System.Xml.Serialization;

namespace COMPASS.Core.Serialization
{
  public class XmlDataDeserializer<T> : IDataDeserializer<T> where T : class
  {
    public T? Deserialize(TextReader reader)
    {
      XmlSerializer serializer = new(typeof(T));
      return serializer.Deserialize(reader) as T;
    }
  }
}
