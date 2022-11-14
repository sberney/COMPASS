using COMPASS.Models;
using System.Windows;
using System.Windows.Controls;

namespace COMPASS.Tools
{
  public class TagTemplateSelector : DataTemplateSelector
  {
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      FrameworkElement elemnt = container as FrameworkElement;
      TreeViewNode node = item as TreeViewNode;
      return node.Tag.IsGroup
    ? elemnt.FindResource("GroupTag") as HierarchicalDataTemplate
    : (DataTemplate)(elemnt.FindResource("RegularTag") as HierarchicalDataTemplate);
    }
  }
}
