using COMPASS.Core.JsonConverters;
using Newtonsoft.Json;

namespace COMPASS.Core
{
  [JsonConverter(typeof(StronglyTypedStringConverter<FilePath>))]
  public struct FilePath : IStronglyTypedString
  {
    public string Value { get; }

    public FilePath(string filepath)
    {
      Value = filepath;
    }

    public static implicit operator FilePath(string filepath)
    {
      return new FilePath(filepath);
    }

    public static implicit operator string(FilePath filepath)
    {
      return filepath.Value;
    }
  }
}
