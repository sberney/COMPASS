namespace COMPASS.Core
{
  public class TagLoader<TColor> : ITagLoader<TColor>
  {
    TreeFlattener TreeFlattener = new();

    public LoadedTags<TColor>? LoadTags(FilePath? tagsFile)
    {
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
    }
  }
}
