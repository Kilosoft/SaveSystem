using UnityEngine;
using System;
using Newtonsoft.Json;

public class Vector2Converter : JsonConverter
{
    private readonly Type type;
    public Vector2Converter(Type type)
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
            var vector = (Vector2)value;
            writer.WriteStartObject();
            writer.WritePropertyName("X");
            writer.WriteValue(vector.x);
            writer.WritePropertyName("Y");
            writer.WriteValue(vector.y);
            writer.WriteEndObject();
        }
    }
}
