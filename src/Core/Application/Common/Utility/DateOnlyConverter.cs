using Newtonsoft.Json;
using System.Globalization;

namespace Squares.Application.Common.Utility;

public class DateOnlyConverter : JsonConverter
{
    public override object? ReadJson(JsonReader reader, Type type, object? existingValue, JsonSerializer serializer)
    {
        try
        {
            DateTime date = DateTime.Parse(reader?.Value?.ToString() ?? string.Empty);
            return DateOnly.FromDateTime(date);
        }
        catch { }
        return Activator.CreateInstance(type);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(((DateOnly?)value)?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }

    public override bool CanConvert(Type type)
    {
        return type == typeof(DateOnly) || type == typeof(DateOnly?);
    }
}