namespace COMPASS.Core.JsonConverters
{
  public class StronglyTypedStringConverterException : Exception
  {
    public StronglyTypedStringConverterException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}