using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPASS.Core
{
  public class TreeFlattener
  {
    // puts all nodes of a tree in a flat enumerable
    public IEnumerable<T> FlattenTree<T>(IEnumerable<T> l) where T : IHasChildren<T>
    {
      List<T> result = new(l);
      for (int i = 0; i < result.Count; i++)
      {
        T parent = result[i];
        foreach (T child in parent.Children)
        {
          result.Add(child);
        }
      }
      return result;
    }
  }
}
