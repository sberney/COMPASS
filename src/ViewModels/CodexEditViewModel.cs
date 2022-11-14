﻿using COMPASS.Models;
using COMPASS.Tools;
using COMPASS.ViewModels.Commands;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;

namespace COMPASS.ViewModels
{
  public class CodexEditViewModel : DealsWithTreeviews, IEditViewModel
  {
    public CodexEditViewModel(Codex toEdit) : base(MVM.CurrentCollection.RootTags)
    {
      EditedCodex = toEdit;
      //apply all changes to new codex so they can be cancelled, only copy changes over after OK is clicked
      TempCodex = new(MVM.CurrentCollection);
      if (!CreateNewCodex) TempCodex.Copy(EditedCodex);

      //Apply right checkboxes in Alltags
      foreach (TreeViewNode t in AllTreeViewNodes)
      {
        t.Expanded = false;
        t.Selected = TempCodex.Tags.Contains(t.Tag);
        if (t.Children.Any(node => TempCodex.Tags.Contains(node.Tag))) t.Expanded = true;
      }
    }

    #region Properties

    readonly Codex EditedCodex;

    private bool CreateNewCodex
    {
      get { return EditedCodex == null; }
    }

    private Codex _tempCodex;
    public Codex TempCodex
    {
      get { return _tempCodex; }
      set { SetProperty(ref _tempCodex, value); }
    }

    private bool CoverArtChanged = false;

    public CreatableLookUpContract Contract { get; set; } = new();

    #endregion

    #region Funtions and Commands

    private ActionCommand _browsePathCommand;
    public ActionCommand BrowsePathCommand => _browsePathCommand ??= new(BrowsePath);
    private void BrowsePath()
    {
      OpenFileDialog openFileDialog = new()
      {
        AddExtension = false,
        InitialDirectory = Path.GetDirectoryName(TempCodex.Path)
      };
      if (openFileDialog.ShowDialog() == true)
      {
        TempCodex.Path = openFileDialog.FileName;
      }
    }

    private ActionCommand _browseURLCommand;
    public ActionCommand BrowseURLCommand => _browseURLCommand ??= new(BrowseURL);
    private void BrowseURL()
    {
      if (CodexViewModel.CanOpenCodexOnline(TempCodex))
        CodexViewModel.OpenCodexOnline(TempCodex);
    }

    private ActionCommand _tagCheckCommand;
    public ActionCommand TagCheckCommand => _tagCheckCommand ??= new(Update_Taglist);
    public void Update_Taglist()
    {
      TempCodex.Tags.Clear();
      foreach (TreeViewNode t in AllTreeViewNodes)
      {
        if (t.Selected)
        {
          TempCodex.Tags.Add(t.Tag);
        }
      }
    }

    private ActionCommand _deleteCodexCommand;
    public ActionCommand DeleteCodexCommand => _deleteCodexCommand ??= new(DeleteCodex);
    private void DeleteCodex()
    {
      if (!CreateNewCodex)
      {
        CodexViewModel.DeleteCodex(EditedCodex);
      }
      CloseAction();
    }

    private ActionCommand _fetchCoverCommand;
    public ActionCommand FetchCoverCommand => _fetchCoverCommand ??= new(FetchCover);
    private void FetchCover()
    {
      CoverFetcher.GetCover(TempCodex);
      RefreshCover();
    }

    private ActionCommand _chooseCoverCommand;
    public ActionCommand ChooseCoverCommand => _chooseCoverCommand ??= new(ChooseCover);
    private void ChooseCover()
    {
      OpenFileDialog openFileDialog = new()
      {
        AddExtension = false,
        Multiselect = false,
        Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
      };
      if (openFileDialog.ShowDialog() == true)
      {
        CoverFetcher.GetCoverFromImage(openFileDialog.FileName, TempCodex);
        RefreshCover();
      }
    }

    private void RefreshCover()
    {
      //force refresh because image is cached
      string CovArt = TempCodex.CoverArt;
      string Thumbn = TempCodex.Thumbnail;
      TempCodex.CoverArt = null;
      TempCodex.Thumbnail = null;
      TempCodex.CoverArt = CovArt;
      TempCodex.Thumbnail = Thumbn;

      CoverArtChanged = true;
    }

    public Action CloseAction { get; set; }

    private ActionCommand _oKCommand;
    public ActionCommand OKCommand => _oKCommand ??= new(OKBtn);
    public void OKBtn()
    {
      //Copy changes into Codex
      if (!CreateNewCodex) EditedCodex.Copy(TempCodex);
      else
      {
        Codex ToAdd = new();
        ToAdd.Copy(TempCodex);
        MVM.CurrentCollection.AllCodices.Add(ToAdd);
        MVM.Refresh();
      }
      //Add new Author and Publishers to lists
      MVM.CurrentCollection.AddAuthors(TempCodex);
      if (TempCodex.Publisher != "" && !MVM.CurrentCollection.PublisherList.Contains(TempCodex.Publisher))
        MVM.CurrentCollection.PublisherList.Add(TempCodex.Publisher);

      //reset needed to show art update
      if (CoverArtChanged) MVM.Refresh();
      CloseAction();
    }

    private ActionCommand _cancelCommand;
    public ActionCommand CancelCommand => _cancelCommand ??= new(Cancel);
    public void Cancel()
    {
      CloseAction();
    }

    #endregion
  }
}

