using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaGridTileMap : MonoBehaviour {

    #region 종류별 타일의 프리팹 참조
    public Dictionary<int, GameObject> dic = new Dictionary<int, GameObject>();
    #endregion

    public Dictionary<Vector2Int, GameObject> Cordination = new Dictionary<Vector2Int, GameObject>();

    //이렇게 하나의 함수로 다 하려고 하지말고, 그냥 타일을 '추가'하는 함수랑, 높이를 변동시키는 함수를 나누자.
    //에디터를 처음 켰을 때 나올 장면에는 그냥 다 노말 타일에 모든 타일이 높이가 1이다.
    //그리고 이렇게 타일 하나 하나를 그릴 때마다 높이를 지정하면 문제가 많다. 모든 타일 정보를 딱 정해놓고, 그에 따라 상대적 높이를 구해야 한다.
    public void DrawOneTile(Vector2Int position, int height) {
        if (!Cordination.ContainsKey(position)) {
            //step 1. 타일 생성
            GameObject addTile = GameObject.Instantiate(dic[0]);

            //step 2. 위치정보 설정
            addTile.GetComponent<HexaTileInfo>().position = position;

            //step 3. 이웃정보 추가

            //step 4. 

            //step 5. 좌표계에 등록
            Cordination[position] = addTile;
        }
        
    }
	public void DrawAllTiles() {

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