using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 타일의 공통 속성을 다루는 클래스
public class HexaTileInfo : MonoBehaviour {
    #region 타일의 외형을 결정하는 속성
    public Vector2Int tilePosition;
    public int tileHeight;//타일의 높이. 4, 5, 6중 하나. 추후의 확장성을 위해 이렇게 함(지금은 3단계로 구분하지만, 나중에 5단계로 구분하고 싶을 수도 있음. 0, 1, 2로 하면 그렇게 하려면 음수가 필요)
    public string userImportTextureUsage;
    #endregion

    #region 맵과 인접 타일의 참조. 0~5 : N, NE, SE, S, SW, NW
    public HexaGridTileMap map;
    public HexaTileInfo[] neighborTile = new HexaTileInfo[6];
    #endregion

    #region 타일의 정보
    public TileType type;
    [HideInInspector]
    public List<HexaTileResourceInfo> resource;
    #endregion

    public static string tempstr;
    [ContextMenu("test_Export")]
    public string ExportJson() {
        try {
            string json = NWJson.ToJson(this);
            Debug.Log(json);
            tempstr = json;
            return json;
        } catch {
            Debug.LogError("HexaTileInfo ExportJosn json make Error");
            return null;
        }
    }
    [ContextMenu("test_import")]
    public void ImportJson() {
        Dictionary<string, object> dic = NWJson.FromJsonToDictionary<string, object>(tempstr);
        Debug.LogError(dic["tilePosition"]);
        Debug.LogError(dic["tileHeight"]);
        Debug.LogError(dic["userImportTextureUsage"]);
        Debug.LogError(dic["map"]);
        Debug.LogError(dic["neighborTile"]);
        Debug.LogError(dic["type"]);
        Debug.LogError(dic["resource"]);

        this.neighborTile = dic["neighborTile"] as HexaTileInfo[];
        Debug.LogError(this.neighborTile);

        var fieldInfos = this.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        List<System.Reflection.FieldInfo> fiList = new List<System.Reflection.FieldInfo>(fieldInfos);
        foreach (var fi in fiList) {
            Debug.LogError(fi.Name);
            if (fi.FieldType == typeof(Vector2Int)) {
                int x = (int)((dic[fi.Name]) as Dictionary<string, object>)["x"];
                int y = (int)((dic[fi.Name]) as Dictionary<string, object>)["y"];
                fi.SetValue(this, new Vector2Int(x, y));
            } else if (fi.FieldType == typeof(HexaTileInfo[])) {
                
            } else {
                fi.SetValue(this, System.Convert.ChangeType(dic[fi.Name], fi.FieldType));
            }
        }
    }
}

public enum TileType {
    City,
    Suburbs,
    Normal,
}