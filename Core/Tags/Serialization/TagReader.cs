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
using Microsoft.Win32.SafeHandles;

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

      //var x = new SafeFileHandle();
      //x.
      using (StreamReader reader = new(FileSystem.FileStream.Create(tagsFile, FileMode.Open, FileAccess.Read, FileShare.Read))) // bufferSize, FileOptions.asynchronous -- replace string with SafeFileHandle?
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
      // todo: map through the list and adapt each, check for nullability failures,
      // and react appropriately to bad data -- by filtering it, or by letting the user know
      //tags.BuildAdapter()
      return tags.Adapt<IReadOnlyList<Tagv2>>();
    }
  }
}
