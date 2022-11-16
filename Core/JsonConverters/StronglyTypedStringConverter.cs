using Newtonsoft.Json;
using System;

namespace COMPASS.Core.JsonConverters
{
  public class StronglyTypedStringConverter<T> : JsonConverter
    where T : struct, IStronglyTypedString
  {
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(T);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType != JsonToken.String)
        return null;

      string? stringRepresentation = serializer.Deserialize(reader, typeof(string)) as string;
      if (stringRepresentation == null)
        return null;

      try
      {
#pragma warning disable CS8605 // Unboxing a possibly null value.
        return (T)Activator.CreateInstance(typeof(T), stringRepresentation);
#pragma warning restore CS8605 // Unboxing a possibly null value.
      }
      catch (Exception e)
      {
        throw new StronglyTypedStringConverterException(
          $"Error in {typeof(StronglyTypedStringConverterException)} while attempting to create and instance of type {typeof(T)} in {nameof(ReadJson)}. Does this type implement a constructor with a single string-typed parameter?",
        e
        );
      }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value is IStronglyTypedString stronglyTypedString)
      {
        writer.WriteValue(stronglyTypedString.Value);
      }
    }
  }
}
