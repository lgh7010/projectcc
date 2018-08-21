using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 타일의 공통 속성을 다루는 클래스
public class HexaTileInfo : MonoBehaviour {
    #region 타일의 외형을 결정하는 속성
    public Vector2Int position;
    public int[] boundaryHeight = new int[6];//타일의 각 모서리의 높이. 0, 1, 2중 하나.
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
}

public enum TileType {
    City,
    Suburbs,
    Normal,
}