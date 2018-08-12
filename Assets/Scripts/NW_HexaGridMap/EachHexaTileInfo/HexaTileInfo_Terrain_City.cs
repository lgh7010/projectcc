using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaTileInfo_Terrain_City : HexaTileInfo {

    public CityInfo cityInfo {
        get {
            return cityInfo;
        }
        set {
            cityInfo = value;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
public class CityInfo {
    public string cityIdx;
    public string cityName;
}