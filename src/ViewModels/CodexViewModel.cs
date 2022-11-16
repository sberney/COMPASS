﻿using COMPASS.Models;
using COMPASS.Tools;
using COMPASS.ViewModels.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace COMPASS.ViewModels
{
  public class CodexViewModel : ViewModelBase
  {
    public CodexViewModel() { }

    //Open Codex whereever
    public static bool OpenCodex(Codex codex)
    {
      bool success = Utils.TryFunctions(MVM.SettingsVM.OpenCodexPriority, codex);
      if (!success)
      {
        _ = MessageBox.Show("Could not open codex, please check local path or URL");
      }

      return success;
    }

    //Open File Offline
    public ReturningRelayCommand<Codex> OpenCodexLocallyCommand => new(OpenCodexLocally, CanOpenCodexLocally);
    public static bool OpenCodexLocally(Codex toOpen)
    {
      if (string.IsNullOrEmpty(toOpen.Path))
      {
        return false;
      }

      try
      {
        _ = Process.Start(new ProcessStartInfo(toOpen.Path) { UseShellExecute = true });
        toOpen.LastOpened = DateTime.Now;
        toOpen.OpenedCount++;
        return true;
      }
      catch (Exception ex)
      {
        Logger.log.Error(ex.InnerException);

        if (toOpen == null)
        {
          return false;
        }

        //Check if folder exists, if not ask users to rename
        string dir = Path.GetDirectoryName(toOpen.Path);
        if (!Directory.Exists(dir))
        {
          string message = $"{toOpen.Path} could not be found. \n" +
          $"If you renamed a folder, go to \n" +
          $"Settings -> General -> Fix Renamed Folder\n" +
          $"to update all references to the old folder name.";
          _ = MessageBox.Show(message, "Path could not be found", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        return false;
      }
    }
    public static bool CanOpenCodexLocally(Codex toOpen)
    {
      return toOpen != null && toOpen.HasOfflineSource();
    }

    //Open File Online
    public ReturningRelayCommand<Codex> OpenCodexOnlineCommand => new(OpenCodexOnline, CanOpenCodexOnline);
    public static bool OpenCodexOnline(Codex toOpen)
    {
      //fails if no internet, pinging 8.8.8.8 DNS instead of server because some sites like gmbinder block ping
      if (!Utils.PingURL())
      {
        return false;
      }

      try
      {
        _ = Process.Start(new ProcessStartInfo(toOpen.SourceURL) { UseShellExecute = true });
        toOpen.LastOpened = DateTime.Now;
        toOpen.OpenedCount++;
        return true;
      }
      catch (Exception ex)
      {
        Logger.log.Error(ex.InnerException);
        return false;
      }

    }
    public static bool CanOpenCodexOnline(Codex toOpen)
    {
      return toOpen != null && toOpen.HasOnlineSource();
    }

    //Open Multiple Files
    public ReturningRelayCommand<IEnumerable> OpenSelectedCodicesCommand => new(OpenSelectedCodices);
    public static bool OpenSelectedCodices(IEnumerable toOpen)
    {
      if (toOpen == null)
      {
        return false;
      }

      List<Codex> ToOpen = toOpen.Cast<Codex>().ToList();
      //MessageBox "Are you Sure?"
      string sMessageBoxText = "You are about to open " + ToOpen.Count + " Files. Are you sure you wish to continue?";
      string sCaption = "Are you Sure?";

      MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
      MessageBoxImage imgMessageBox = MessageBoxImage.Warning;

      MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, imgMessageBox);

      if (rsltMessageBox == MessageBoxResult.Yes)
      {
        foreach (Codex f in ToOpen)
        {
          _ = OpenCodex(f);
        }

        return true;
      }
      else { return false; }
    }

    //Edit File
    public RelayCommand<Codex> EditCodexCommand => new(EditCodex);
    public static void EditCodex(Codex toEdit)
    {
      //MVM.CurrentEditViewModel = new CodexEditViewModel(toEdit);
      FilePropWindow fpw = new(new CodexEditViewModel(toEdit));
      _ = fpw.ShowDialog();
      fpw.Topmost = true;
    }

    public RelayCommand<Codex> FavoriteCodexCommand => new(FavoriteCodex);
    public static void FavoriteCodex(Codex toFavorite)
    {
      toFavorite.Favorite = toFavorite.Favorite switch
      {
        true => false,
        false => true
      };
    }

    //Edit Multiple files
    public RelayCommand<IEnumerable> EditCodicesCommand => new(EditCodices);
    public static void EditCodices(IEnumerable toEdit)
    {
      if (toEdit == null)
      {
        return;
      }

      List<Codex> ToEdit = toEdit.Cast<Codex>().ToList();
      FileBulkEditWindow fpw = new(new CodexBulkEditViewModel(ToEdit));
      _ = fpw.ShowDialog();
      fpw.Topmost = true;
    }

    //Show in Explorer
    public RelayCommand<Codex> ShowInExplorerCommand => new(ShowInExplorer, CanOpenCodexLocally);
    public static void ShowInExplorer(Codex toShow)
    {
      string folderPath = Path.GetDirectoryName(toShow.Path);
      ProcessStartInfo startInfo = new()
      {
        Arguments = folderPath,
        FileName = "explorer.exe"
      };
      _ = Process.Start(startInfo);
    }


    //Move Codex to other CodexCollection
    public RelayCommand<object[]> MoveToCollectionCommand => new(MoveToCollection);
    public static void MoveToCollection(object[] par)
    {
      //par contains 2 parameters
      List<Codex> ToMoveList = new();
      string targetCollectionName;

      //extract Collection parameter
      if (par[0] != null)
      {
        targetCollectionName = (string)par[0];
      }
      else
      {
        return;
      }

      if (targetCollectionName == MVM.CurrentCollectionName)
      {
        return;
      }

      //extract Codex parameter
      if ((par[1] as Codex) != null)
      {
        ToMoveList.Add((Codex)par[1]);
      }
      else
      {
        IList list = par[1] as IList;
        ToMoveList = list.Cast<Codex>().ToList();
      }

      //MessageBox "Are you Sure?"
      string MessageSingle = "Moving " + ToMoveList[0].Title + " to " + targetCollectionName + " will remove all tags from the Codex, are you sure you wish to continue?";
      string MessageMultiple = "Moving these " + ToMoveList.Count + " files to " + targetCollectionName + " will remove all tags from the Codices, are you sure you wish to continue?";

      string sCaption = "Are you Sure?";
      string sMessageBoxText = ToMoveList.Count == 1 ? MessageSingle : MessageMultiple;

      MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
      MessageBoxImage imgMessageBox = MessageBoxImage.Warning;

      MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, imgMessageBox);

      if (rsltMessageBox == MessageBoxResult.Yes)
      {
        CodexCollection TargetCollection = new(targetCollectionName);
        foreach (Codex ToMove in ToMoveList)
        {
          ToMove.Tags.Clear();
          // Give file new ID and move it to other folder
          ToMove.Id = Utils.GetAvailableID(TargetCollection.AllCodices);

          //Add Codex to target CodexCollection
          TargetCollection.AllCodices.Add(ToMove);

          //Update Author and Publisher List
          TargetCollection.AddAuthors(ToMove);
          if (ToMove.Publisher != "" && !TargetCollection.PublisherList.Contains(ToMove.Publisher))
          {
            TargetCollection.PublisherList.Add(ToMove.Publisher);
          }

          //Move cover art to right folder with new ID
          string newCoverArt = CodexCollection.CollectionsPath + targetCollectionName + @"\CoverArt\" + ToMove.Id + ".png";
          string newThumbnail = CodexCollection.CollectionsPath + targetCollectionName + @"\Thumbnails\" + ToMove.Id + ".png";
          File.Copy(ToMove.CoverArt, newCoverArt);
          File.Copy(ToMove.Thumbnail, newThumbnail);

          //Delete file in original folder
          MVM.CurrentCollection.DeleteCodex(ToMove);
          MVM.CollectionVM.RemoveCodex(ToMove);

          //Update the cover art metadata to new path, has to happen after delete so old one gets deleted
          ToMove.CoverArt = newCoverArt;
          ToMove.Thumbnail = newThumbnail;
        }
        //Save changes to TargetCollection
        TargetCollection.SaveCodices();
      }
    }

    //Delete Codex
    public RelayCommand<object> DeleteCodexCommand => new(DeleteCodex);
    public static void DeleteCodex(object o)
    {
      //works for Single codex and list
      List<Codex> ToDeleteList = new();

      // if single codex, add to list
      if ((o as Codex) != null)
      {
        ToDeleteList.Add(o as Codex);
      }
      // if already list, cast is as such
      else
      {
        IList list = o as IList;
        ToDeleteList = list.Cast<Codex>().ToList();
      }
      //Actually delete stuff
      foreach (Codex ToDelete in ToDeleteList)
      {
        MVM.CurrentCollection.DeleteCodex(ToDelete);
        MVM.CollectionVM.RemoveCodex(ToDelete);
      }
      MVM.Refresh();
    }
  }
}
