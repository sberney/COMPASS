using System.IO;

namespace COMPASS.Core.Tags
{
    public interface IDataDeserializer<T> where T : class
    {
        T? Deserialize(TextReader reader);
    }
}