namespace COMPASS.Core.JsonConverters
{
  public interface IStronglyTypedString
  {
    /// <summary>
    /// A struct that implements this, that provides a constructor
    /// taking a single string-typed parameter and assigns it to Value,
    /// may decorate itself with [JsonConverter(typeof(StronglyTypedStringConverter{T}))]
    // in order to enable automatically serialization to and from a string type.
    /// </summary>
    public interface IStronglyTypedString
    {
      public string Value { get; }
    }
  }
}