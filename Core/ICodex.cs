using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace COMPASS.Core
{
  public interface ICodex<TColor>
  {
    ObservableCollection<string> Authors { get; set; }
    string AuthorsAsString { get; }
    string CoverArt { get; set; }
    DateTime DateAdded { get; set; }
    string Description { get; set; }
    bool Favorite { get; set; }
    int ID { get; set; }
    string ISBN { get; set; }
    DateTime LastOpened { get; set; }
    int OpenedCount { get; set; }
    int PageCount { get; set; }
    string Path { get; set; }
    bool Physically_Owned { get; set; }
    string Publisher { get; set; }
    int Rating { get; set; }
    DateTime? ReleaseDate { get; set; }
    string SerializableSortingTitle { get; set; }
    string SortingTitle { get; set; }
    string SourceURL { get; set; }
    List<int> TagIDs { get; set; }
    ObservableCollection<ITag<TColor>> Tags { get; set; }
    string Thumbnail { get; set; }
    string Title { get; set; }
    string Version { get; set; }

    // void Copy(Codex c);
    bool HasOfflineSource();
    bool HasOnlineSource();
  }
}