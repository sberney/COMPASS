using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPASS.Core
{
  public class FreshIdCreator
  {
    public int Create<T>(IEnumerable<T> collection) where T : IHasId
    {
      int tempID = 0;
      while (collection.Any(f => f.Id == tempID))
      {
        tempID++;
      }
      return tempID;
    }
  }
}
