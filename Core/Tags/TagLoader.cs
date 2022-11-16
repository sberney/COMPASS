using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace COMPASS.Core.Tags
{
  public class TagLoader : ITagLoader
  {
    TreeFlattener TreeFlattener = new();

    public LoadedTags? LoadTags(FilePath? tagsFile)
    {
      throw new NotImplementedException("asdf");
      /*
      if (tagsFile == null)
      {
        return null;
      }

      if (!File.Exists(tagsFile.ToString()))
      {
        return new LoadedTags<TColor>
        {
          Root = new List<ITag<TColor>>(),
          All = new List<ITag<TColor>>(),
        };
      }

      //loading root tags          
      using (StreamReader Reader = new(tagsFile))
      {
        System.Xml.Serialization.XmlSerializer serializer = new(typeof(List<ITag<TColor>>));
        var rootTags = serializer.Deserialize(Reader) as List<ITag<TColor>>;

        //Constructing AllTags and pass it to all the tags
        var allTags = TreeFlattener.FlattenTree(rootTags).ToList();
        foreach (var t in allTags)
        {
          t.AllTags = AllTags;
        }
      }
      */
    }
  }
}
