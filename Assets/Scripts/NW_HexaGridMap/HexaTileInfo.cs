using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaTileInfo : MonoBehaviour {
    #region 타일의 외형을 결정하는 속성
    public Vector2Int position;
    public int[] boundaryHeight = new int[6];//타일의 외곽 높이. -2, -1, 0, 1, 2중 하나
    public string userImportTextureUsage;
    #endregion

    #region 맵과 인접 타일의 참조. 0~5 : N, NE, SE, S, SW, NW
    public HexaGridTileMap map;
    public HexaTileInfo[] neighborTile = new HexaTileInfo[6];
    #endregion

    #region 타일의 정보
    public Type_Usage type_usage;
    public Type_Terrain type_terrain;
    [HideInInspector]
    public List<HexaTileResourceInfo> resource;
    #endregion
}

public enum Type_Usage {
    City,
    Suburbs,
    Resource,
    Normal,
}
public enum Type_Terrain {
    Plane,
    Mountain,
    Coast,
    See,
}