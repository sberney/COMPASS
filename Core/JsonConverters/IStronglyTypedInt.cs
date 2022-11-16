namespace COMPASS.Core.JsonConverters
{
    /// <summary>
    /// A struct that implements this, that provides a constructor
    /// taking a single int-typed parameter and assigns it to Value,
    /// may decorate itself with [JsonConverter(typeof(StronglyTypedIntConverter{T}))]
    // in order to enable automatically serialization to and from a int type.
    /// </summary>
    public interface IStronglyTypedInt : IStronglyTypedAlias<int>
  {
  }
}