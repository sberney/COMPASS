using BlackPearl.Controls.Contract;

namespace COMPASS.Tools
{
  //Enables multiselect combobox to create items
  public class CreatableLookUpContract : ILookUpContract
  {
    public bool SupportsNewObjectCreation => true;

    public object CreateObject(object sender, string searchString)
    {
      return searchString.Trim();
    }

    public bool IsItemEqualToString(object sender, object item, string seachString)
    {
      return item is string
&& string.Compare(seachString.ToLowerInvariant().Trim(), (item as string).ToLowerInvariant().Trim(), System.StringComparison.InvariantCultureIgnoreCase) == 0;
    }

    public bool IsItemMatchingSearchString(object sender, object item, string searchString)
    {
      return item as string is not null
&& (string.IsNullOrEmpty(searchString) || ((string)item).ToLower().Trim().Contains(searchString.ToLower().Trim()));
    }
  }
}
