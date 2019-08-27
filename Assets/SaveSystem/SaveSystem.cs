using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public abstract class ISaveItem
    {
        public string Key { get; set; }
    }

    public class SaveItem<T> : ISaveItem
    {
        public T item;
    }

    private static Dictionary<string, ISaveItem> items;

    private static bool isInit;

    public static void Init()
    {
        if (!isInit)
        {
            isInit = true;
            items = new Dictionary<string, ISaveItem>();
            Load();
        }
    }

    public static Dictionary<string, ISaveItem> GetItems()
    {
        return items;
    }

    public static void Load()
    {
        var json = PlayerPrefs.GetString("Save");
        var save = JsonConvert.DeserializeObject<Dictionary<string, ISaveItem>>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        if (save != null) items = save;
    }

    public static void Save()
    {
        if (!isInit) Init();
        var json = JsonConvert.SerializeObject(items, new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            Converters = new List<JsonConverter>
            {
                new ColorConverter(typeof(Color)),
                new Color32Converter(typeof(Color32)),
                new Vector2Converter(typeof(Vector2)),
                new Vector3Converter(typeof(Vector3))
            }
        });
        PlayerPrefs.SetString("Save", json);
    }

    public static void Set<T>(string key, T item, bool commitSave = true)
    {
        if (!isInit) Init();
        if (!items.ContainsKey(key)) items.Add(key, null);
        items[key] = new SaveItem<T>() { item = item, Key = key };
        if (commitSave)
        {
            Save();
        }
    }

    public static void Remove(string key)
    {
        if (!isInit) Init();
        if (items.ContainsKey(key)) items.Remove(key);
        Save();
    }

    public static T Get<T>(string key)
    {
        if (!isInit) Init();
        var result = default(T);
        if (items.ContainsKey(key))
        {
            try
            {
                result = (items[key] as SaveItem<T>).item;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error key: {key}:" + e.ToString());
            }
        }
        return result;
    }

}

public class Color32Converter : JsonConverter
{
    private readonly Type type;
    public Color32Converter(Type type)
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
            var color = (Color32)value;
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
