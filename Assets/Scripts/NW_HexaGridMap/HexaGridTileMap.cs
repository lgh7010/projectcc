using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaGridTileMap : MonoBehaviour {

    #region 종류별 타일의 프리팹 참조
    public Dictionary<int, GameObject> dic = new Dictionary<int, GameObject>();//주변 타일의 height(1자리 자연수)를 북쪽 타일부터 시계방향으로 나열한 int값이 키가 된다. (Ex : 455456)
    #endregion

    public Dictionary<Vector2Int, HexaTileInfo> Cordination = new Dictionary<Vector2Int, HexaTileInfo>();

    public void AddTile(Vector2Int position, int height) {
        if (!Cordination.ContainsKey(position)) {
            Debug.LogError("Cordination doesn't have key : " + position);
            return;
        }

        //step 1. 타일 생성
        GameObject addTile = GameObject.Instantiate(dic[111111]);
        addTile.transform.SetParent(this.gameObject.transform);

        //step 2. 기본 타일 컴포넌트 추가.
        HexaTileInfo cp = addTile.AddComponent<HexaTileInfo_Normal>();

        //step 3. 위치정보 설정
        cp.tilePosition = position;
        cp.tileHeight = 5;

        //step 4. 이웃정보 추가
        try {
            cp.neighborTile[0] = this.Cordination[position + new Vector2Int(0, 2)];//null이면 그냥 null이 들어가면 된다.
            cp.neighborTile[1] = this.Cordination[position + new Vector2Int(1, 1)];
            cp.neighborTile[2] = this.Cordination[position + new Vector2Int(1, -1)];
            cp.neighborTile[3] = this.Cordination[position + new Vector2Int(0, -2)];
            cp.neighborTile[4] = this.Cordination[position + new Vector2Int(-1, -1)];
            cp.neighborTile[5] = this.Cordination[position + new Vector2Int(-1, 1)];
        } catch {
            Debug.LogError("HexaGridTileMap addTile, set neighborTiles Error");
            GameObject.Destroy(addTile);
            return;
        }

        //step 5. 기타 필요한 참조 확보 및 정보 추가
        cp.map = this;
        cp.type = TileType.Normal;

        //step 6. 좌표계에 등록
        try {
            Cordination[position] = cp;
        } catch {
            Debug.LogError("HexaGridTileMap addTile, set Cordination Error");
            GameObject.Destroy(addTile);
            return;
        }
    }
    public void ChangeTileType(Vector2Int position, TileType type) {
        if (!Cordination.ContainsKey(position)) {
            Debug.LogError("Cordination doesn't have key : " + position);
            return;
        }
        if(Cordination[position].type == type) {
            Debug.Log("바꾸려는 타일 타입이, 이전의 타일 타입과 동일합니다.");
            return;
        }

        //타일 타입을 바꾸면, 공통속성(HexaTileInfo클래스의 속성)만 보존된다.
        
    }
    public void ChnageTileHeight(Vector2Int position, int delta) {
        if (!Cordination.ContainsKey(position)) {
            Debug.LogError("Cordination doesn't have key : " + position);
            return;
        }
        if(Cordination[position].tileHeight + delta > 6 || Cordination[position].tileHeight + delta < 4) {
            Debug.LogError("HexaTile height change Error. key : " + position);
            return;
        }

        try {
            Cordination[position].tileHeight += delta;
        } catch {
            Debug.LogError("HexaTile ChnageTileHeight Error. key : " + position + ", before height : " + Cordination[position].tileHeight + ". delta : " + delta);
            return;
        }
    }
    public void AddResourceToTile(Vector2Int position, HexaTileResourceInfo rsc) {
        if (!Cordination.ContainsKey(position)) {
            Debug.LogError("Cordination doesn't have key : " + position);
            return;
        }

        try {
            if(Cordination[position].resource == null) {
                Cordination[position].resource = new List<HexaTileResourceInfo>();
            }
            Cordination[position].resource.Add(rsc);
        } catch {
            Debug.LogError("HexaTile AddResourceToTile Error. key : " + position);
            return;
        }
    }
    
}

public static class HexaTileCalculateUtil {
    public static int GetTileKey(int[] edgeHeights) {
        string temp = "";
        for(int i = 0; i < 6; i++) {
            temp += edgeHeights[i];
        }
        return int.Parse(temp);
    }
}

/*
 * 맵 에디터에는 높이 1의 타일이 기본적으로 펼쳐져 있다(정하는 크기만큼_그냥 이거 밖에다 덧붙이는것도 가능)
 * (DrawOneTile함수를 이용해서, 주어진 크기를 인터폴레이션 하여 내부를 그냥 일반 타일로 싹 그리는것 뿐. 그러니 당연히 그 밖에다가 추가도 가능)
 * 
 * 맵 딕셔너리에는 각 타일인포만 저장하면 되고, 타일인포에는 각각의 타일의 각 모서리 높이 데이터가 들어있으므로, 이걸 이용해 랜더링만 수행한다.
 * 브러시로 맵을 재단하는것은 만들기도 힘들거니와 만들어도 비 직관적이고 불편하다. 그냥 타일하나를 브러시의 최소 단위로 하고,
 * 브러시를 통해 '높이'만 조절하면 랜더링 함수에서 알아서 '높이'에 따라 적절한 타일을 가져다 이니시에이트 해주면 된다.
 * 
 * 타일의 각 모서리의 높이가 0, 1, 2중 하나이므로, 타일의 각 모서리를 0, 1, 2, 3, 4, 5 번째 자리로 하고, 모서리의 높이를 표시하면, 겹치지 않는 일종의 '타일 키'를 얻을 수 있다.
 * Ex : 각 모서리의 높이가 0, 0, 1, 1, 0, 1인 타일의 키는 그냥 001101 -> int값 1101
 * 각 모서리의 높이가 0, 1, 2, 1, 0, 0 -> 12100
 * */