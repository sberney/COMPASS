﻿using COMPASS.Models;
using COMPASS.Tools;
using COMPASS.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace COMPASS.ViewModels
{
  public class CodexBulkEditViewModel : DealsWithTreeviews, IEditViewModel
  {
    public CodexBulkEditViewModel(List<Codex> ToEdit) : base(MVM.CurrentCollection.RootTags)
    {
      EditedCodices = ToEdit;
      TempCodex = new(MVM.CurrentCollection);

      //set common metadata
      _commonAuthors = EditedCodices.Select(f => f.Authors.ToList()).Aggregate((xs, ys) => xs.Intersect(ys).ToList());
      TempCodex.Authors = new(_commonAuthors);
      if (EditedCodices.All(f => f.Publisher == EditedCodices[0].Publisher))
      {
        TempCodex.Publisher = EditedCodices[0].Publisher;
      }

      if (EditedCodices.All(f => f.Rating == EditedCodices[0].Rating))
      {
        TempCodex.Rating = EditedCodices[0].Rating;
      }

      if (EditedCodices.All(f => f.Physically_Owned == EditedCodices[0].Physically_Owned))
      {
        TempCodex.Physically_Owned = EditedCodices[0].Physically_Owned;
      }

      if (EditedCodices.All(f => f.ReleaseDate == EditedCodices[0].ReleaseDate))
      {
        TempCodex.ReleaseDate = EditedCodices[0].ReleaseDate;
      }
    }

    #region Properties

    private readonly List<Codex> EditedCodices;

    //True if adding tags, false if removing
    private bool _tagMode = true;
    public bool TagMode
    {
      get => _tagMode;
      set => SetProperty(ref _tagMode, value);
    }


    private ObservableCollection<Tag> _tagsToAdd = new();
    private ObservableCollection<Tag> _tagsToRemove = new();

    public ObservableCollection<Tag> TagsToAdd
    {
      get => _tagsToAdd;
      set => SetProperty(ref _tagsToAdd, value);
    }

    public ObservableCollection<Tag> TagsToRemove
    {
      get => _tagsToRemove;
      set => SetProperty(ref _tagsToRemove, value);
    }

    private Codex _tempCodex;
    public Codex TempCodex
    {
      get => _tempCodex;
      set => SetProperty(ref _tempCodex, value);
    }

    public CreatableLookUpContract Contract { get; set; } = new();

    public List<string> _commonAuthors;

    #endregion

    #region Functions and Commands

    private RelayCommand<bool> _setTagModeCommand;
    public RelayCommand<bool> SetTagModeCommand => _setTagModeCommand ??= new(SetTagMode);
    public void SetTagMode(bool tagMode)
    {
      TagMode = tagMode;

      // Apply right checkboxes in Alltags
      if (TagMode)
      {
        foreach (TreeViewNode t in AllTreeViewNodes)
        {
          t.Expanded = false;
          t.Selected = TagsToAdd.Contains(t.Tag);
          if (t.Children.Any(node => TagsToAdd.Contains(node.Tag)))
          {
            t.Expanded = true;
          }
        }
      }

      else
      {
        foreach (TreeViewNode t in AllTreeViewNodes)
        {
          t.Expanded = false;
          t.Selected = TagsToRemove.Contains(t.Tag);
          if (t.Children.Any(node => TagsToRemove.Contains(node.Tag)))
          {
            t.Expanded = true;
          }
        }
      }
    }

    private ActionCommand _tagCheckCommand;
    public ActionCommand TagCheckCommand => _tagCheckCommand ??= new(Update_Taglist);
    public void Update_Taglist()
    {
      if (TagMode)
      {
        TagsToAdd.Clear();
      }
      else
      {
        TagsToRemove.Clear();
      }

      foreach (TreeViewNode t in AllTreeViewNodes)
      {
        if (t.Selected)
        {
          if (TagMode)
          {
            TagsToAdd.Add(t.Tag);
          }
          else
          {
            TagsToRemove.Add(t.Tag);
          }
        }
      }
    }


    public Action CloseAction { get; set; }

    public ActionCommand _oKCommand;
    public ActionCommand OKCommand => _oKCommand ??= new(OKBtn);
    public void OKBtn()
    {
      //Copy changes into each Codex
      //delete authors that were deleted
      IEnumerable<string> deletedAuthors = _commonAuthors.Except(TempCodex.Authors);
      IEnumerable<string> addedAuthors = TempCodex.Authors.Except(_commonAuthors);
      MVM.CurrentCollection.AddAuthors(TempCodex);
      foreach (Codex f in EditedCodices)
      {
        //add new ones
        foreach (string author in addedAuthors)
        {
          if (!f.Authors.Contains(author))
          {
            f.Authors.Add(author);
          }
        }
        //remove deleted ones
        foreach (string author in deletedAuthors)
        {
          _ = f.Authors.Remove(author);
        }
      }

      if (TempCodex.Publisher is not "" and not null)
      {
        foreach (Codex f in EditedCodices)
        {
          f.Publisher = TempCodex.Publisher;
        }
      }

      if (TempCodex.Physically_Owned == true)
      {
        foreach (Codex f in EditedCodices)
        {
          f.Physically_Owned = true;
        }
      }

      if (TempCodex.Rating > 0)
      {
        foreach (Codex f in EditedCodices)
        {
          f.Rating = TempCodex.Rating;
        }
      }

      if (TempCodex.Version != null)
      {
        foreach (Codex f in EditedCodices)
        {
          f.Version = TempCodex.Version;
        }
      }

      if (TempCodex.ReleaseDate != null)
      {
        foreach (Codex f in EditedCodices)
        {
          f.ReleaseDate = TempCodex.ReleaseDate;
        }
      }

      //Add new Publishers to lists
      if (TempCodex.Publisher != "" && !MVM.CurrentCollection.PublisherList.Contains(TempCodex.Publisher))
      {
        MVM.CurrentCollection.PublisherList.Add(TempCodex.Publisher);
      }

      //Add and remove Tags
      foreach (Codex f in EditedCodices)
      {
        if (TagsToAdd.Count > 0)
        {
          //add all tags from TagsToAdd
          foreach (Tag t in TagsToAdd)
          {
            f.Tags.Add(t);
          }
          //remove duplacates
          f.Tags = new ObservableCollection<Tag>(f.Tags.Distinct());
        }
        if (TagsToRemove.Count > 0)
        {
          //remove Tags from TagsToRemove
          foreach (Tag t in TagsToRemove)
          {
            _ = f.Tags.Remove(t);
          }
        }

      }
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

