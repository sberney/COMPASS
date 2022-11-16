using COMPASS.Core.Tags;
using COMPASS.Models;
using COMPASS.Tools;
using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace COMPASS.ViewModels
{
  public abstract class DealsWithTreeviews : ViewModelBase, IDropTarget
  {
    public DealsWithTreeviews(List<ITag> treeRoot)
    {
      TreeRoot = treeRoot;
      RefreshTreeView();
    }

    private List<ITag> TreeRoot { get; set; }

    //TreeViewSource with hierarchy
    private ObservableCollection<TreeViewNode> _treeviewsource;
    public ObservableCollection<TreeViewNode> TreeViewSource
    {
      get => _treeviewsource;
      set => SetProperty(ref _treeviewsource, value);
    }

    //AllTreeViewNodes without hierarchy for iterating purposes
    private HashSet<TreeViewNode> _alltreeViewNodes;
    public HashSet<TreeViewNode> AllTreeViewNodes
    {
      get => _alltreeViewNodes;
      set => SetProperty(ref _alltreeViewNodes, value);
    }

    public ObservableCollection<TreeViewNode> CreateTreeViewSource(List<ITag> rootTags)
    {
      ObservableCollection<TreeViewNode> newRootNodes = new();
      foreach (ITag t in rootTags)
      {
        newRootNodes.Add(ConvertTagToTreeViewNode(t));
      }

      return newRootNodes;
    }

    public List<ITag> ExtractTagsFromTreeViewSource(ObservableCollection<TreeViewNode> treeViewSource)
    {
      List<ITag> newRootTags = new();
      foreach (TreeViewNode n in treeViewSource)
      {
        newRootTags.Add(ConvertTreeViewNodeToTag(n));
      }
      foreach (var t in newRootTags)
      {
        t.ParentId = -1;
      }
      return newRootTags;
    }
    private TreeViewNode ConvertTagToTreeViewNode(ITag t)
    {
      TreeViewNode Result = new(t);
      foreach (ITag t2 in t.Children)
      {
        Result.Children.Add(ConvertTagToTreeViewNode(t2));
      }

      return Result;
    }

    private ITag ConvertTreeViewNodeToTag(TreeViewNode node)
    {
      ITag Result = node.Tag;
      //clear childeren the tag thinks it has
      Result.Children.Clear();

      //add childeren accodring to treeview
      foreach (TreeViewNode childnode in node.Children)
      {
        Result.Children.Add(ConvertTreeViewNodeToTag(childnode));
      }
      //set partentID for all the childeren
      foreach (var childtag in Result.Children)
      {
        childtag.ParentId = Result.Id;
      }

      return Result;
    }

    public void RefreshTreeView()
    {
      TreeViewSource = CreateTreeViewSource(TreeRoot);
      AllTreeViewNodes = Utils.FlattenTree(TreeViewSource).ToHashSet();
    }


    //Drop on Treeview Behaviour
    void IDropTarget.DragOver(IDropInfo dropInfo)
    {
      DragDrop.DefaultDropHandler.DragOver(dropInfo);
    }
    void IDropTarget.Drop(IDropInfo dropInfo)
    {
      DragDrop.DefaultDropHandler.Drop(dropInfo);
      //cannot do TreeRoot = ExtractTagsFromTreeViewSource(TreeViewSource); because that changes ref of TreeRoot
      TreeRoot.Clear();
      TreeRoot.AddRange(ExtractTagsFromTreeViewSource(TreeViewSource));
    }
    /*** End of Treeview section ***/
  }
}
