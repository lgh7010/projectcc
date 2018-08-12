using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaTileInfo : MonoBehaviour {
    /// <summary>
    /// 해당 타일의 위치입니다. 씬의 좌표값이 아니라 HexaTile 좌표계의 좌표이며, 따라서 x, z 값이 모두 정수여야 합니다.
    /// </summary>
    public Vector2 position {
        get {
            return position;
        }
        set {
            position = value;
        }
    }
    /// <summary>
    /// 해당 타일의 사용처입니다.
    /// </summary>
    public string usage {
        get {
            return usage;
        }
        set {
            usage = value;
        }
    }
    /// <summary>
    /// 해당 타일의 종류입니다.
    /// </summary>
    public TileType type {
        get {
            return type;
        }
        set {
            type = value;
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public enum TileType {
    //월드뷰의 타일 종류
    Terrain_World,
    City,

    //도시뷰의 타일 종류
    Terrain_City,
    Building,
    Resource,
}
