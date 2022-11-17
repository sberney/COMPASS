using COMPASS.Core.Serialization;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Windows.Documents;
using Mapster;

namespace COMPASS.Core.Tags.Serialization
{
  public class TagReader : ITagReader
  {
    protected readonly IFileSystem FileSystem;
    protected IDataDeserializer<List<SerializableTag>> Deserializer = new XmlDataDeserializer<List<SerializableTag>>();
    protected TreeFlattener TreeFlattener = new();

    public TagReader(IFileSystem fileSystem)
    {
      FileSystem = fileSystem;
    }

    // Creates System.IO based reader
    public TagReader() : this(fileSystem: new FileSystem()) { }

    public SerializableTags? ReadTags(FilePath tagsFile)
    {
      if (!FileSystem.File.Exists(tagsFile))
        return null;

      using (StreamReader reader = new(FileSystem.FileStream.Create(tagsFile, FileMode.Open)))
      {
        var rootTags = Deserializer.Deserialize(reader);
        if (rootTags is null)
          return null;

        var convertedRootTags = ConvertTags(rootTags);

        var flatTags = TreeFlattener.FlattenTree(convertedRootTags).ToList();
        return new SerializableTags
        {
          Root = convertedRootTags,
          All = flatTags
        };
      }
    }

    protected IReadOnlyList<ITag> ConvertTags(List<SerializableTag> tags)
    {
      return tags.Adapt<IReadOnlyList<Tagv2>>();
    }
  }
}
