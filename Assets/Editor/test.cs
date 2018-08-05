using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class test {

    [MenuItem("Test/MakeJsonFromDictionary")]
    public static void func() {
        Dictionary<string, BVT_DATA> dic = new Dictionary<string, BVT_DATA>();
        BVT_DATA temp1 = new BVT_DATA();
        temp1.ID = "id";
        temp1.DataList = new List<OTHER_DATA>();
        temp1.DataList.Add(new OTHER_DATA());
        temp1.DataDic = new Dictionary<string, OTHER_DATA>();
        temp1.DataDic.Add("key1", new OTHER_DATA());
        dic.Add("key0", temp1);

        //step 1.
        string json = NWJson.ToJson(dic);//이렇게 사용하면 된다.
        Debug.LogError(json);

        //step 2.
        Dictionary<string, BVT_DATA> dic2 = NWJson.FromJsonToDictionary<string, BVT_DATA>(json);//이렇게 사용하면 된다.
        foreach (KeyValuePair<string, BVT_DATA> kv in dic2) {
            Debug.LogError("key : " + kv.Key + ", value : " + kv.Value.ID);
        }

        //step 3.
        string json2 = NWJson.ToJson(dic2);
        Debug.LogError(json2);
    }
}

[Serializable]
public class BVT_DATA {
    public string ID;
    public List<OTHER_DATA> DataList;
    public Dictionary<string, OTHER_DATA> DataDic;

    public BVT_DATA() {
        ID = "id";
        DataList = new List<OTHER_DATA>();
        DataDic = new Dictionary<string, OTHER_DATA>();
    }
}
public class OTHER_DATA {
    public string NAME;

    public OTHER_DATA() {
        NAME = "name";
    }
}