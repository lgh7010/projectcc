using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 타일의 공통 속성을 다루는 클래스
public class HexaTileInfo : MonoBehaviour {
    #region 타일의 외형을 결정하는 속성
    public Vector2Int tilePosition;
    public int tileHeight;//타일의 높이. 4, 5, 6중 하나. 추후의 확장성을 위해 이렇게 함(지금은 3단계로 구분하지만, 나중에 5단계로 구분하고 싶을 수도 있음. 0, 1, 2로 하면 그렇게 하려면 음수가 필요)
    public string userImportTextureUsage;
    public GameObject faceModel;
    public GameObject bodyModel;
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
    
    public static HexaTileInfo ExportCommonAttributes(HexaTileInfo source) {
        HexaTileInfo ret = new HexaTileInfo();
        ret.tilePosition = source.tilePosition;
        ret.tileHeight = source.tileHeight;
        ret.userImportTextureUsage = source.userImportTextureUsage;
        ret.faceModel = source.faceModel;
        ret.bodyModel = source.bodyModel;
        ret.map = source.map;
        ret.neighborTile = source.neighborTile;
        ret.type = source.type;
        ret.resource = source.resource;
        return ret;
    }
    public void ImportCommonAttributes(HexaTileInfo source) {
        this.tilePosition = source.tilePosition;
        this.tileHeight = source.tileHeight;
        this.userImportTextureUsage = source.userImportTextureUsage;
        this.faceModel = source.faceModel;
        this.bodyModel = source.bodyModel;
        this.map = source.map;
        this.neighborTile = source.neighborTile;
        this.type = source.type;
        this.resource = source.resource;
    }
}

public enum TileType {
    City,
    Suburbs,
    Normal,
}