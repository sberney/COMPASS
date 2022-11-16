using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace COMPASS.Core
{
  public interface IHasChildren<out T> where T : IHasChildren<T>
  {
    public IReadOnlyList<T> Children { get; }
  }
}
