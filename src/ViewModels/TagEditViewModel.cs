using COMPASS.Core;
using COMPASS.Models;
using COMPASS.ViewModels.Commands;
using System;
using IWinTag = COMPASS.Core.ITag<System.Windows.Media.Color>;

namespace COMPASS.ViewModels
{
  public class TagEditViewModel : ViewModelBase, IEditViewModel
  {
    public TagEditViewModel(IWinTag toEdit) : base()
    {
      var tagCreator = new TagCreator(MVM.CurrentCollection.AllTags);

      EditedTag = toEdit;
      var isCreating = toEdit == null;
      if (isCreating)
      {
        CreateNewTag = true;
        TempTag = tagCreator.CreateFresh();
      }
      else
      {
        TempTag = tagCreator.CreateCopyFrom(EditedTag);
      }

      ShowColorSelection = false;

      //Commands
      CloseColorSelectionCommand = new ActionCommand(CloseColorSelection);
    }

    #region Properties

    private IWinTag EditedTag;
    private readonly bool CreateNewTag;

    //TempTag to work with
    private IWinTag tempTag;
    public IWinTag TempTag
    {
      get => tempTag;
      set => SetProperty(ref tempTag, value);
    }

    //visibility of Color Selection
    private bool showcolorselection = false;
    public bool ShowColorSelection
    {
      get => showcolorselection;
      set
      {
        _ = SetProperty(ref showcolorselection, value);
        RaisePropertyChanged(nameof(ShowInfoGrid));
      }
    }

    //visibility of General Info Selection
    public bool ShowInfoGrid
    {
      get => !ShowColorSelection;
      set { }
    }

    #endregion

    #region Functions and Commands

    private ActionCommand _oKCommand;
    public ActionCommand OKCommand => _oKCommand ??= new(OKBtn);
    public void OKBtn()
    {
      var tagCreator = new TagCreator(MVM.CurrentCollection.AllTags);

      if (CreateNewTag)
      {
        EditedTag = tagCreator.CreateFresh();
        if (TempTag.ParentID == -1)
        {
          MVM.CurrentCollection.RootTags.Add(EditedTag);
        }
      }
      else
      {
        EditedTag = tagCreator.CreateCopyFrom(TempTag);
      }

      // Apply changes 
      MVM.TFViewModel.TagsTabVM.RefreshTreeView();

      if (CreateNewTag)
      {
        MVM.CurrentCollection.AllTags.Add(EditedTag);
        // reset fields
        tagCreator = new TagCreator(MVM.CurrentCollection.AllTags);  // probably unnecessary to reininitialize here due to pass-by-reference, but not certain
        TempTag = tagCreator.CreateFresh();
        EditedTag = null;
      }
      else
      {
        CloseAction();
      }
    }

    private ActionCommand _cancelCommand;
    public ActionCommand CancelCommand => _cancelCommand ??= new(Cancel);
    public void Cancel()
    {
      if (!CreateNewTag)
      {
        CloseAction();
      }
      else
      {
        TempTag = new Tag(MVM.CurrentCollection.AllTags);
      }
      EditedTag = null;
    }

    public ActionCommand CloseColorSelectionCommand { get; private set; }
    public Action CloseAction { get; set; }

    private void CloseColorSelection()
    {
      ShowColorSelection = false;
    }

    #endregion
  }
}
