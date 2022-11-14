using COMPASS.Models;
using COMPASS.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace COMPASS.Views
{
  /// <summary>
  /// Interaction logic for Tag_FilterView.xaml
  /// </summary>
  public partial class Tag_FilterView : UserControl
  {
    public Tag_FilterView()
    {
      InitializeComponent();
    }
    public void TagTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      TreeViewNode selectednode = (TreeViewNode)e.NewValue;
      if (selectednode == null) return;
      if (selectednode.Tag.IsGroup) return;
      ViewModelBase.MVM.CollectionVM.AddTagFilter(selectednode.Tag);
      selectednode.Selected = false;
    }

    private void TagTree_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

      if (treeViewItem != null)
      {
        ((TagsFiltersViewModel)DataContext).TagsTabVM.Context = ((TreeViewNode)treeViewItem.Header).Tag as Tag;
        e.Handled = true;
      }
    }

    static TreeViewItem VisualUpwardSearch(DependencyObject source)
    {
      while (source != null && source is not TreeViewItem)
        source = VisualTreeHelper.GetParent(source);

      return source as TreeViewItem;
    }
  }
}
