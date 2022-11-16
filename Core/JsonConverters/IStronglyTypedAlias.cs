namespace COMPASS.Core.JsonConverters
{
    /// <summary>
    /// A struct that implements this, that provides a constructor
    /// taking a single T-typed parameter and assigns it to Value,
    /// may decorate itself with [JsonConverter(typeof(StronglyTypedAliasConverter{V, T}))]
    // in order to enable automatically serialization to and from a T-type.
    /// </summary>
    public interface IStronglyTypedAlias<T>
  {
    T Value { get; }
  }
}