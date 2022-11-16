using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Windows.Documents;

namespace COMPASS.Core.Tags
{
  public class TagReader : ITagReader
  {
    protected readonly IFileSystem FileSystem;
    protected IDataDeserializer<Collection<Tagv2>> Deserializer = new XmlDataDeserializer<Collection<Tagv2>>();
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
