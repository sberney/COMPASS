﻿using COMPASS.Tools;
using COMPASS.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using IWinTag = COMPASS.Core.ITag<System.Windows.Media.Color>;

namespace COMPASS.Models
{
  public class CodexCollection : ObservableObject
  {
    public CodexCollection(string collectionDirectory)
    {
      DirectoryName = collectionDirectory;

      LoadTags();
      LoadCodices();

      Properties.Settings.Default.StartupCollection = collectionDirectory;
    }

    public static readonly string CollectionsPath = Constants.CompassDataPath + @"\Collections\";

    #region Properties
    private string _directoryName;
    public string DirectoryName
    {
      get => _directoryName;
      set => SetProperty(ref _directoryName, value);
    }

    public string CodicesDataFilePath => CollectionsPath + DirectoryName + @"\CodexInfo.xml";
    public string TagsDataFilepath => CollectionsPath + DirectoryName + @"\Tags.xml";

    //Tag Lists
    public List<IWinTag> AllTags { get; private set; } = new();
    public List<IWinTag> RootTags { get; set; }

    //File Lists
    public ObservableCollection<Codex> AllCodices { get; private set; } = new();

    //Metadata Lists
    private ObservableCollection<string> _authorList = new();
    public ObservableCollection<string> AuthorList
    {
      get => _authorList;
      set => SetProperty(ref _authorList, value);
    }

    private ObservableCollection<string> _publisherList = new();
    public ObservableCollection<string> PublisherList
    {
      get => _publisherList;
      set => SetProperty(ref _publisherList, value);
    }
    #endregion

    #region Load Data From File
    //Loads the RootTags from a file and constructs the Alltags list from it
    private void LoadTags()
    {
      if (File.Exists(TagsDataFilepath))
      {
        //loading root tags          
        using (StreamReader Reader = new(TagsDataFilepath))
        {
          System.Xml.Serialization.XmlSerializer serializer = new(typeof(List<Tag>));
          RootTags = serializer.Deserialize(Reader) as List<IWinTag>;
        }

        //Constructing AllTags and pass it to all the tags
        AllTags = Utils.FlattenTree(RootTags).ToList();
        foreach (Tag t in AllTags)
        {
          t.AllTags = AllTags;
        }
      }
      else
      {
        RootTags = new();
      }
    }

    //Loads AllCodices list from Files
    private void LoadCodices()
    {
      if (File.Exists(CodicesDataFilePath))
      {
        using (StreamReader Reader = new(CodicesDataFilePath))
        {
          System.Xml.Serialization.XmlSerializer serializer = new(typeof(ObservableCollection<Codex>));
          AllCodices = serializer.Deserialize(Reader) as ObservableCollection<Codex>;
        }


        foreach (Codex f in AllCodices)
        {
          //Populate Author and Publisher List
          AddAuthors(f);
          if (!string.IsNullOrEmpty(f.Publisher) && !PublisherList.Contains(f.Publisher))
          {
            PublisherList.Add(f.Publisher);
          }

          //reconstruct tags from ID's
          foreach (int id in f.TagIDs)
          {
            f.Tags.Add(AllTags.First(t => t.ID == id));
          }

          //apply sorting titles
          f.SortingTitle = f.SerializableSortingTitle;
        }
        //Sort them
        AuthorList = new(AuthorList.OrderBy(n => n));
        PublisherList = new(PublisherList.OrderBy(n => n));
      }
      else
      {
        AllCodices = new();
      }
    }
    #endregion

    #region Save Data To XML File

    public void SaveTags()
    {
      using XmlWriter writer = XmlWriter.Create(TagsDataFilepath, SettingsViewModel.XmlWriteSettings);
      System.Xml.Serialization.XmlSerializer serializer = new(typeof(List<Tag>));
      serializer.Serialize(writer, RootTags);
    }

    public void SaveCodices()
    {
      //Copy id's of tags into list for serialisation
      foreach (Codex codex in AllCodices)
      {
        codex.TagIDs = codex.Tags.Select(t => t.ID).ToList();
      }

      using XmlWriter writer = XmlWriter.Create(CodicesDataFilePath, SettingsViewModel.XmlWriteSettings);
      System.Xml.Serialization.XmlSerializer serializer = new(typeof(ObservableCollection<Codex>));
      serializer.Serialize(writer, AllCodices);
    }

    #endregion

    public void DeleteCodex(Codex Todelete)
    {
      //Delete file from all lists
      _ = AllCodices.Remove(Todelete);

      //Delete Coverart & Thumbnail
      File.Delete(Todelete.CoverArt);
      File.Delete(Todelete.Thumbnail);
    }

    public void DeleteTag(IWinTag todel)
    {
      //Recursive loop to delete all childeren
      if (todel.Children.Count > 0)
      {
        DeleteTag(todel.Children[0]);
        DeleteTag(todel);
      }
      _ = AllTags.Remove(todel);
      //remove from parent items list
      _ = todel.ParentID == -1 ? RootTags.Remove(todel) : todel.GetParent().Children.Remove(todel);
    }

    public void RenameCollection(string NewCollectionName)
    {
      foreach (Codex codex in AllCodices)
      {
        //Replace folder names in image paths, include leading and ending "\" to avoid replacing wrong things
        codex.CoverArt = codex.CoverArt.Replace(@"\" + DirectoryName + @"\", @"\" + NewCollectionName + @"\");
        codex.Thumbnail = codex.Thumbnail.Replace(@"\" + DirectoryName + @"\", @"\" + NewCollectionName + @"\");
      }
      Directory.Move(CollectionsPath + DirectoryName, CollectionsPath + NewCollectionName);
      DirectoryName = NewCollectionName;
    }

    public void AddAuthors(Codex codex)
    {
      foreach (string author in codex.Authors)
      {
        if (!string.IsNullOrEmpty(author) && !AuthorList.Contains(author))
        {
          AuthorList.Add(author);
        }
      }
    }
  }
}
