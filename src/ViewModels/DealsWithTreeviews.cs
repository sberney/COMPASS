﻿using COMPASS.Models;
using COMPASS.Tools;
using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IWinTag = COMPASS.Core.ITag<System.Windows.Media.Color>;

namespace COMPASS.ViewModels
{
  public abstract class DealsWithTreeviews : ViewModelBase, IDropTarget
  {
    public DealsWithTreeviews(List<IWinTag> treeRoot)
    {
      TreeRoot = treeRoot;
      RefreshTreeView();
    }

    private List<IWinTag> TreeRoot { get; set; }

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

    public ObservableCollection<TreeViewNode> CreateTreeViewSource(List<IWinTag> rootTags)
    {
      ObservableCollection<TreeViewNode> newRootNodes = new();
      foreach (IWinTag t in rootTags)
      {
        newRootNodes.Add(ConvertTagToTreeViewNode(t));
      }

      return newRootNodes;
    }

    public List<IWinTag> ExtractTagsFromTreeViewSource(ObservableCollection<TreeViewNode> treeViewSource)
    {
      List<IWinTag> newRootTags = new();
      foreach (TreeViewNode n in treeViewSource)
      {
        newRootTags.Add(ConvertTreeViewNodeToTag(n));
      }
      foreach (var t in newRootTags)
      {
        t.ParentID = -1;
      }
      return newRootTags;
    }
    private TreeViewNode ConvertTagToTreeViewNode(IWinTag t)
    {
      TreeViewNode Result = new(t);
      foreach (IWinTag t2 in t.Children)
      {
        Result.Children.Add(ConvertTagToTreeViewNode(t2));
      }

      return Result;
    }

    private IWinTag ConvertTreeViewNodeToTag(TreeViewNode node)
    {
      IWinTag Result = node.Tag;
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
        childtag.ParentID = Result.ID;
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
