using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using UnityEngine;

public class NWJson {
    public static string ToJson(object obj) {
        return JsonWriter.Serialize(obj);
    }
    public static Dictionary<K, T> FromJsonToDictionary<K, T>(string json) {
        return JsonReader.Deserialize<Dictionary<K, T>>(json);
    }
    public static List<T> FromJsonToList<T>(string jsonString) {
        return JsonReader.Deserialize<List<T>>(jsonString);
    }
}