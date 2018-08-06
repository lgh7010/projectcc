using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using UnityEngine;

public class NWJson {
    public static string ToJson(object obj) {
        JsonWriter writer = new JsonWriter();
        return writer.Write(obj);
    }
    public static Dictionary<K, T> FromJsonToDictionary<K, T>(string json) {
        JsonReader reader = new JsonReader();
        return reader.Read<Dictionary<K, T>>(json);
    }
    public static List<T> FromJsonToList<T>(string json) {
        JsonReader reader = new JsonReader();
        return reader.Read<List<T>>(json);
    }
}