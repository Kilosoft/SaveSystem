using UnityEngine;
using System;
using Newtonsoft.Json;

public class ColorConverter : JsonConverter
{
    private readonly Type type;
    public ColorConverter(Type type)
    {
        this.type = type;
    }

    public override bool CanConvert(Type objectType)
    {
        return type == objectType;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value.GetType() == type)
        {
            var color = (Color)value;
            writer.WriteStartObject();
            writer.WritePropertyName("R");
            writer.WriteValue(color.r);
            writer.WritePropertyName("G");
            writer.WriteValue(color.g);
            writer.WritePropertyName("B");
            writer.WriteValue(color.b);
            writer.WritePropertyName("A");
            writer.WriteValue(color.a);
            writer.WriteEndObject();
        }
    }
}
