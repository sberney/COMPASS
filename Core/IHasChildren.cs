using System.Collections.ObjectModel;

namespace COMPASS.Core
{
  public interface IHasChildren<T> where T : IHasChildren<T>
  {
    public ObservableCollection<T> Children { get; set; }
  }
}
