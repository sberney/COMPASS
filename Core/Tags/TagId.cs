using COMPASS.Core.JsonConverters;
using Newtonsoft.Json;

namespace COMPASS.Core.Tags
{
  public struct TagId : IStronglyTypedInt
  {
    public int Value { get; }

    public TagId(int tagId)
    {
      Value = tagId;
    }

    public static implicit operator TagId(int tagId)
    {
      return new TagId(tagId);
    }

    public static implicit operator int(TagId tagId)
    {
      return tagId.Value;
    }
  }
}
