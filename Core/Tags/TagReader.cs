using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;

namespace COMPASS.Core.Tags
{
  public class TagReader : ITagReader
  {
    protected IDataDeserializer<IReadOnlyList<Tagv2>> Deserializer = new XmlDataDeserializer<IReadOnlyList<Tagv2>>();
    protected TreeFlattener TreeFlattener = new();

    public SerializableTags? ReadTags(FilePath tagsFile)
    {
      if (!File.Exists(tagsFile))
        return null;

      using (StreamReader reader = new(tagsFile))
      {
        var rootTags = Deserializer.Deserialize(reader) as IReadOnlyList<ITag>;
        if (rootTags is null)
          return null;

        var flatTags = TreeFlattener.FlattenTree(rootTags).ToList();
        return new SerializableTags
        {
          Root = rootTags,
          All = flatTags
        };
      }
    }
  }
}
